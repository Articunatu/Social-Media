# Social Media

A full-stack social media application built as a personal project to explore clean architecture patterns, CQRS, and modern web development practices.

## Features

- Create, view, and delete posts
- Comment on posts
- React to posts
- User profiles with post history
- Photo uploads
- JWT-based authentication (no ASP.NET Identity)
- Paginated feed

## Tech Stack

### Backend (.NET 9 / C#)
| Concern | Choice |
|---|---|
| Framework | ASP.NET Core 9 — Minimal API |
| Architecture | Clean Architecture + DDD + CQRS (MediatR) |
| Database | Microsoft SQL Server via Entity Framework Core |
| Authentication | JSON Web Tokens (custom, no Identity package) |
| Validation | FluentValidation + EF Core configurations |
| Testing | xUnit, FluentAssertions, EF Core InMemory |

### Frontend (Next.js 15 / TypeScript)
| Concern | Choice |
|---|---|
| Framework | React 19 via Next.js 15 |
| Styling | Tailwind CSS + DaisyUI |
| Server state | TanStack Query v5 |
| HTTP client | Axios |

## Project Structure

```
code/
├── client/          # Next.js frontend
└── server/
    └── SocialMedia/
        ├── SM.Domain/                      # Entities, value objects, domain logic
        ├── SM.Application/                 # CQRS commands/queries, business logic
        ├── SM.Infrastructure/              # Cross-cutting concerns (auth, behaviors)
        ├── SM.Persistence/                 # EF Core DbContext, configurations, repositories
        ├── SM.WebApi/                      # Minimal API endpoints, middleware
        ├── SM.Domain.UnitTests/
        ├── SM.Application.UnitTests/
        └── SM.Application.IntegrationTests/
```

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- Microsoft SQL Server (local or Docker)

### Backend

1. Update the connection string in `SM.WebApi/appsettings.Development.json`.
2. Apply migrations:
   ```bash
   cd code/server/SocialMedia
   dotnet ef database update --project SM.Persistence --startup-project SM.WebApi
   ```
3. Run the API:
   ```bash
   dotnet run --project SM.WebApi
   ```
   The API starts on `https://localhost:5001` and Swagger is available in development.

### Frontend

```bash
cd code/client
npm install
npm run dev
```

The app starts on `http://localhost:3000` and expects the API at `http://localhost:5001`.

## Architecture Notes

The backend follows Clean Architecture with four layers: **Domain**, **Application**, **Infrastructure**, and **Persistence** (presentation is the Web API project).

**No repository pattern** — EF Core's `DbContext` is injected directly into command/query handlers. This is an intentional design decision based on the argument that EF Core already provides a sufficient abstraction layer. The trade-off is that integration tests use the EF Core InMemory provider rather than mocks, which means some SQL-specific behaviour (constraints, transactions) is not fully exercised in tests.

> **Note:** There is code in progress to migrate integration tests to use [test containers](https://testcontainers.com/) for SQL Server instead of InMemoryDb. This will make tests both more reliable and more readable, as they will run against a real database engine.

CQRS is implemented via **MediatR** — every operation is either a `Command` (mutating) or a `Query` (read-only), keeping handler logic focused and independently testable.

**Caching and logging** are auto-injected into the MediatR pipeline using custom pipeline behaviors. This means all requests and responses can be logged and cached transparently, without polluting business logic or handler code.
