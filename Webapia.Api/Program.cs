using System.Reflection;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Webapia.Api.ExceptionHandlers;
using Webapia.Application.Common.Errors.DTOs;
using Webapia.Application.Features.Products.Interfaces;
using Webapia.Application.Features.Products.Services;
using Webapia.Infrastructure;
using Webapia.Infrastructure.Data;
using Webapia.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);


// ====================================================================================
// === Database ===

// Register DataSourceOptions with DI
builder.Services.Configure<DataSourceOptions>(
    builder.Configuration.GetSection(DataSourceOptions.SectionName));

// Read immediately for conditional service registration
var dataSourceOptions = builder.Configuration
    .GetSection(DataSourceOptions.SectionName)
    .Get<DataSourceOptions>() ?? new DataSourceOptions();

var healthChecksBuilder = builder.Services.AddHealthChecks();

if (dataSourceOptions.Provider == DataProvider.Mock)
{
    builder.Services.AddSingleton<InMemoryProductRepository>();
    builder.Services.AddScoped<IProductRepository>(sp =>
        sp.GetRequiredService<InMemoryProductRepository>());
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<EfProductRepository>();
    builder.Services.AddScoped<IProductRepository, EfProductRepository>();

    healthChecksBuilder.AddDbContextCheck<AppDbContext>();
}

// ERROR HANDLER: routes caught exceptions through GlobalExceptionHandler (IExceptionHandler)
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Versioning & Swagger
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Webapia API v1", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "Webapia API v2", Version = "v2" });

    // Feed the compiler-generated XML doc comments into Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// controllers specifications
builder.Services.AddControllers(options =>
    {
        options.Filters.Add(new ProducesAttribute("application/json")); // all responses are JSON
    })
    .ConfigureApiBehaviorOptions(options =>
    {
// ERROR HANDLER: input model validation
        options.InvalidModelStateResponseFactory = context =>
        {
            var validationErrors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var errorResponse = new ErrorResponseDto(
                StatusCodes.Status400BadRequest,
                "One or more validation errors occurred.",
                validationErrors
            );

            return new BadRequestObjectResult(errorResponse);
        };
    });

// Register Application services
builder.Services.AddScoped<IProductService, ProductService>();

// ====================================================================================

var app = builder.Build();

// ====================================================================================

// Database Migrations
if (dataSourceOptions.Provider == DataProvider.Database)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// === Pipeline Middlewares ===

// Uses custom GlobalExceptionHandler
app.UseExceptionHandler();

// ERROR HANDLER: Generates ErrorResponseDto payloads for empty 4xx/5xx responses (e.g., 404, 405)
app.UseStatusCodePages(async statusCodeContext =>
{
    var httpContext = statusCodeContext.HttpContext;
    var statusCode = httpContext.Response.StatusCode;

    var message = statusCode switch
    {
        404 => "The requested endpoint does not exist.",
        405 => "The HTTP method is not supported for this endpoint.",
        _ => "An error occurred handling your request."
    };

    var errorDto = new ErrorResponseDto(statusCode, message);

    await httpContext.Response.WriteAsJsonAsync(errorDto);
});

// Swagger docs
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Webapia API v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Webapia API v2");
    });
}

// === Map Endpoints ===

// all controller based endpoints
app.MapControllers();

// db health check endpoint
app.MapHealthChecks("/health");

// ====================================================================================


app.Run();