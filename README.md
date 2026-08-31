# Webapia

A versioned REST API for managing eshop products, built with ASP.NET Core following Clean Architecture principles.

## Overview

Webapia exposes product data through a versioned REST API:

- **v1** — list all products, get a product by id, update a product's description
- **v2** — same as v1, plus paginated product listing (default page size 10)

The solution is split into four layers, each in its own project, following Clean Architecture:

```
Webapia.Domain          → Entities, domain exceptions (no dependencies on anything else)
Webapia.Application     → DTOs, service interfaces, business logic (services)
Webapia.Infrastructure  → EF Core DbContext, migrations, repository implementations
Webapia.Api             → Controllers, exception handling, DI wiring (Program.cs)
```

Each layer has a matching unit test project (`Webapia.Domain.UnitTests`, `Webapia.Application.UnitTests`,
`Webapia.Api.UnitTests`), plus a shared `Webapia.TestCommon` project for reusable test helpers.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (latest LTS)
- [Docker](https://www.docker.com/) (only required if running against a real SQL Server database — see below)
- An IDE: Visual Studio, VS Code, or JetBrains Rider

## Running the application

The app supports two data source modes, controlled by a single config value — no code changes required to switch between
them.

### Option A — Mock data (no database required)

This is the fastest way to run the project. Set the provider to `Mock` in `Webapia.Api/appsettings.json` (or
`appsettings.Development.json`):

```json
{
  "DataSource": {
    "Provider": "Mock"
  }
}
```

Then run:

```bash
dotnet run --project Webapia.Api
```

In this mode, an in-memory repository is used, pre-seeded with mock product data. No SQL Server, no Docker, no
migrations needed.

### Option B — Real database (SQL Server via Docker)

Set the provider to `Database`:

```json
{
  "DataSource": {
    "Provider": "Database"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=WebapiaDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True"
  }
}
```

Start a local SQL Server instance:

```bash
docker-compose up -d
```

Then run the app:

```bash
dotnet run --project Webapia.Api
```

Pending EF Core migrations are applied automatically on startup (`db.Database.Migrate()`), and the database is seeded
with initial product data — no manual migration step required.

> **Note on the committed connection string:** the password above is a throwaway local development credential, committed
> for reviewer convenience so the project runs with zero setup friction. In a real production deployment, this would
> instead come from environment variables, User Secrets (local dev), or a secrets manager (Azure Key Vault, AWS Secrets
> Manager, etc.) — never committed to source control.

## API documentation

Once the app is running (in the `Development` environment, which is the default when using `dotnet run`), Swagger UI is
available at:

```
https://localhost:{port}/swagger
```

Swagger is versioned — use the version selector in the top-right of the UI to switch between the **v1** and **v2** API
documents. Each endpoint includes a summary, parameter descriptions, and documented response codes.

## API endpoints

| Version | Method | Route                                 | Description                                     |
|---------|--------|----------------------------------------|-------------------------------------------------|
| v1      | GET    | `/api/v1/products`                    | List all products                               |
| v1      | GET    | `/api/v1/products/{id}`               | Get a product by id                             |
| v1      | PATCH  | `/api/v1/products/{id}/description`   | Update a product's description                  |
| v2      | GET    | `/api/v2/products?page=1&pageSize=10` | List products, paginated (default page size 10) |
| v2      | GET    | `/api/v2/products/{id}`               | Get a product by id                             |
| v2      | PATCH  | `/api/v2/products/{id}/description`   | Update a product's description                  |

A health check endpoint is also available at `/health`.

## Error handling

Every error path in the API — regardless of where it originates — is returned in a single consistent shape:

```json
{
  "statusCode": 404,
  "message": "Product not found.",
  "timeStamp": "2026-08-31 10:34:11"
}
```

For model-validation failures, the shape additionally includes a per-field breakdown via the optional `errors` property:

```json
{
  "statusCode": 400,
  "message": "One or more validation errors occurred.",
  "timeStamp": "2026-08-31 10:34:11",
  "errors": {
    "dto": ["The dto field is required."]
  }
}
```

There is no single mechanism that catches every kind of error — ASP.NET Core surfaces failures at different, independent
stages of the request pipeline, so each stage is handled explicitly:

| Error source | Mechanism | Status codes |
|---|---|---|
| Domain exceptions (`NotFoundException`, `BadRequestException`) and any unhandled exception | `GlobalExceptionHandler` (`IExceptionHandler`, registered via `AddExceptionHandler`) | 400, 404, 500 |
| Invalid/malformed request bodies | Custom `InvalidModelStateResponseFactory`, configured via `ConfigureApiBehaviorOptions` | 400 |
| Requests to a route that doesn't exist | `UseStatusCodePages` | 404 |
| Requests using an HTTP method not supported by an otherwise-valid route | `UseStatusCodePages` | 405 |

All four converge on the same `ErrorResponseDto` shape, so API consumers never need to handle more than one error
contract.

## Running the tests

The solution includes three unit test projects, one per testable layer:

```bash
dotnet test
```

This runs all test projects (`Webapia.Domain.UnitTests`, `Webapia.Application.UnitTests`, `Webapia.Api.UnitTests`)
across the solution.

To run a single project:

```bash
dotnet test Webapia.Application.UnitTests
```

Test coverage includes:

- **Domain** — custom exception constructors and inheritance contracts
- **Application** — `ProductService` business logic (not-found handling, pagination mapping, description updates), and
  entity-to-DTO mapping
- **Api** — controller behavior (correct service calls, correct status codes) for both API versions, and
  `GlobalExceptionHandler`'s behavior for each exception type

Mocking is done with Moq; assertions use FluentAssertions.

## Database migrations

Migrations live in `Webapia.Infrastructure/Migrations`. To add a new migration after changing the EF Core model:

```bash
dotnet ef migrations add <MigrationName> -p Webapia.Infrastructure -s Webapia.Api
```

This works regardless of the `DataSource:Provider` setting in `appsettings.json`, via a design-time
`IDesignTimeDbContextFactory` that constructs the `DbContext` independently of the app's runtime DI configuration.

## Known limitations / possible improvements

These were consciously scoped out for this exercise, and are noted here rather than left unaddressed:

- **Rate limiting** is not implemented. For a public-facing deployment, this would be added via
  `Microsoft.AspNetCore.RateLimiting`, partitioned by client/API key.
- **HTTPS redirection** is not enforced, for local development simplicity. It could be added via e.g. Caddy or the
  standard `UseHttpsRedirection`/HSTS middleware for a real deployment.
- **Integration tests** (exercising the full HTTP pipeline via `WebApplicationFactory`, including real model binding,
  versioning, and database interaction) are not included — the current test suite is unit tests only, each layer tested
  in isolation with mocked dependencies.