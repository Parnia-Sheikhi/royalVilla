# Royal Villa

A villa management app I built with ASP.NET Core. It's split into three projects: a REST API, an MVC web app that talks to the API, and a small shared library for the DTOs.

I made it to get comfortable with structuring a .NET solution in layers instead of putting everything in one project.

## Projects

- `royalVilla.API` – the Web API (controllers, EF Core, JWT auth)
- `RoyalVillaWeb` – the MVC front end (Bootstrap views, cookie login)
- `RoyalVilla.DtO` – DTOs shared by both projects

## What it does

- Register and log in. The API returns a JWT, and the web app keeps the user logged in with a cookie.
- Two roles: `Admin` and `Customer`. The villa create, edit and delete endpoints require the Admin role.
- CRUD for villas (name, details, rate, sqft, occupancy, image).
- CRUD for villa amenities. Each villa can have many amenities.
- Every API response comes back in the same `ApiResponse<T>` shape, so success and error handling on the web side stays simple.

## Tech

- ASP.NET Core (.NET 10), Web API + MVC
- Entity Framework Core with SQL Server, code-first migrations
- AutoMapper for entity <-> DTO mapping
- JWT bearer authentication
- API versioning and Scalar for the API docs
- Bootstrap for the UI

## Running it

1. Clone the repo.
2. Set `ConnectionStrings:DefaultConnection` in `royalVilla.API/appsettings.json` to your own SQL Server.
3. Set a JWT secret under `JwSettings:Secret` (I'd use `dotnet user-secrets` for this instead of committing it).
4. Run `royalVilla.API`. Migrations are applied on startup, so there's no need to run `update-database`.
5. Make sure `ServiceUrls:VillaAPI` in `RoyalVillaWeb/appsettings.json` matches the address the API is running on.
6. Run `RoyalVillaWeb`.

In development the API docs are at `/scalar`.

## Next steps

- Password hashing
- Tests
- API v2
