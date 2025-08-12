# DocumentManager

DocumentManager is a multi-project ASP.NET Core solution for managing documents, employees and related entities.

## Projects
- **DocumentManager.API** – Web API configured with Entity Framework Core, AutoMapper, CORS and Swagger.
- **DocumentManager.MVC** – ASP.NET Core MVC front-end with Identity and Razor Pages.
- **DocumentManager.DAL** – Data access layer containing the Entity Framework Core context and models.

## Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/) 6.0 or later
- SQL Server instance for the `DefaultConnection` connection string

## Getting Started
1. Restore and build the solution:
   ```bash
   dotnet build
   ```
2. Run the API project:
   ```bash
   dotnet run --project DocumentManager.API
   ```
3. Run the MVC project:
   ```bash
   dotnet run --project DocumentManager.MVC
   ```

The API exposes Swagger UI at `/swagger` and allows cross-origin requests from the MVC app.

## Database
The `ApplicationDbContext` defines tables for departments, employees, document formats and more using Entity Framework Core.
