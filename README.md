Royal Villa

A full-stack villa booking and management platform built with a layered ASP.NET Core architecture — a versioned REST API on one side and an MVC web frontend on the other, sharing a common DTO library.

Overview

Royal Villa lets customers browse and view villas, while admins manage the full catalog (villas + amenities) through role-protected endpoints. The project was built to practice a production-style layered .NET architecture: separating the API, the presentation layer, and the data-transfer contracts into independent projects instead of one monolithic app.
Structure: royalVilla.API (REST API) / RoyalVillaWeb (MVC frontend) / RoyalVilla.DtO (shared DTOs)

Features
JWT authentication with role-based authorization (Admin, Customer)
Versioned REST API (v1/v2), documented with Scalar
Full CRUD for villas and villa amenities (with a one-to-many relationship between them)
AutoMapper for mapping between EF Core entities and DTOs
A standardized ApiResponse<T> wrapper so every endpoint returns a consistent success/error shape
EF Core Code-First migrations against SQL Server
MVC frontend with cookie-based auth, consuming the API through a typed HttpClient
Tech Stack

Backend: ASP.NET Core Web API, Entity Framework Core, SQL Server, AutoMapper, JWT Bearer Authentication, Scalar (API docs) Frontend: ASP.NET Core MVC, Bootstrap, Cookie Authentication Tooling: EF Core Migrations, API Versioning

Getting Started
Prerequisites
.NET SDK
SQL Server (LocalDB or full instance)
Setup
Clone the repo
Update the DefaultConnection string in royalVilla.API/appsettings.json
Run the API project — migrations are applied automatically on startup
Update ServiceUrls:VillaAPI in RoyalVillaWeb/appsettings.json to point to the running API
Run the RoyalVillaWeb project

The API docs (Scalar) are available at /scalar in development mode.

Known Limitations
Passwords are currently stored in plain text swapping in BCrypt or ASP.NET Identity for password hashing is a planned improvement
No automated tests yet
