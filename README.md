# MPP.Back API

Template base de API REST con arquitectura por capas y autenticación JWT.

## Stack
- .NET 8 / ASP.NET Core
- EF Core 8 + SQL Server
- Swagger (OpenAPI)
- Serilog (console + file)
- FluentValidation
- JWT Bearer Auth
- Rate limiting global
- API Versioning (URL segment)

## Estructura
- MPP.Back.API: Endpoints, middlewares, configuración (Swagger, JWT, CORS, RateLimiter, Serilog)
- MPP.Back.Application: DTOs, servicios de negocio, validaciones
- MPP.Back.Domain: Entidades de dominio
- MPP.Back.Infrastructure: EF Core, repositorios, JWT generator
- MPP.Back.Shared: extensiones y marcadores DI

## Configuración
Archivo principal: `MPP.Back.API/appsettings.json`.

Claves relevantes:
- ConnectionStrings:DefaultConnection
- Database:ConnectionStringName
- Jwt:Key, Jwt:Issuer, Jwt:Audience, Jwt:ExpirationMinutes
- Cors:PolicyName, Cors:Origins
- Swagger:Title, Swagger:Description, Swagger:RoutePrefix
- RateLimiter:PermitLimit, RateLimiter:PermitLimitGlobalBySeconds, RateLimiter:RetryAfterSeconds
- AuditLogging:Enabled, LogRequestBody, LogResponseBody, BodySizeLimit, ExcludedPaths

## Requisitos
- .NET SDK 8
- SQL Server 
- Entity Framework

## Base URL y Swagger
- HTTPS: `https://localhost:7085`

## Migraciones
1. `Add-Migration c -OutputDir "Contexts/Migrations"`
2. `Update-Database c`

## Endpoints (v1)
- `POST /api/v1/login`
- `GET /api/v1/productos`
- `GET /api/v1/productos/{productoId}`
- `POST /api/v1/productos` (rol `Admin`)
- `PUT /api/v1/productos/{productoId}` (rol `Admin`)
- `DELETE /api/v1/productos/{productoId}` (rol `Admin`)
