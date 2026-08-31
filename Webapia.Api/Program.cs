using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Webapia.Application.Features.Products.Interfaces;
using Webapia.Application.Features.Products.Services;
using Webapia.Infrastructure;
using Webapia.Infrastructure.Data;
using Webapia.Infrastructure.Repositories;
using Webapia.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Webapia API v1", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "Webapia API v2", Version = "v2" });

    // Feed the compiler-generated XML doc comments (see .csproj) into Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Register DataSourceOptions with DI so other classes can inject IOptions<DataSourceOptions>
builder.Services.Configure<DataSourceOptions>(
    builder.Configuration.GetSection(DataSourceOptions.SectionName));

// Read immediately to decide repository/DbContext registration below
var dataSourceOptions = builder.Configuration
    .GetSection(DataSourceOptions.SectionName)
    .Get<DataSourceOptions>() ?? new DataSourceOptions();

var healthChecksBuilder = builder.Services.AddHealthChecks();

// decide the db source & register
if (dataSourceOptions.Provider == DataProvider.Mock)
{
    // Mock mode: no SQL Server needed at all. Singleton so seeded/added
    // data is shared and visible across requests for the app's lifetime.
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

// register Services
builder.Services.AddScoped<IProductService, ProductService>();

// BUILD THE APP
var app = builder.Build();

// migate db
if (dataSourceOptions.Provider == DataProvider.Database)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// show docs when dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Webapia API v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Webapia API v2");
    });
}

// use Middlewares for request processing
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();