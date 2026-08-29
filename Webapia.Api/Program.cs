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
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Webapia API v1",
        Version = "v1"
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Webapia API v2",
        Version = "v2"
    });
});

// Read the data source setting once so we can branch registration below.
builder.Services.Configure<DataSourceOptions>(
    builder.Configuration.GetSection(DataSourceOptions.SectionName));

var dataSourceOptions = builder.Configuration
    .GetSection(DataSourceOptions.SectionName)
    .Get<DataSourceOptions>() ?? new DataSourceOptions();

var healthChecksBuilder = builder.Services.AddHealthChecks();

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

    // Only meaningful when AppDbContext is actually registered.
    healthChecksBuilder.AddDbContextCheck<AppDbContext>();
}

builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// migate db
if (dataSourceOptions.Provider == DataProvider.Database)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Webapia API v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Webapia API v2");
    });
}

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();