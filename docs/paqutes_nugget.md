# 📦 Paquetes NuGet por Proyecto

## 🧠 TallerCore (Dev 1)

### Base de datos

| Paquete | Comando | Descripción |
|---|---|---|
| `Microsoft.EntityFrameworkCore.SqlServer` | `dotnet add package Microsoft.EntityFrameworkCore.SqlServer` | Proveedor EF Core para SQL Server. Permite usar `AppDbContext`. |
| `Microsoft.EntityFrameworkCore.Tools` | `dotnet add package Microsoft.EntityFrameworkCore.Tools` | Habilita los comandos `add-migration` y `update-database`. |
| `Microsoft.EntityFrameworkCore.Design` | `dotnet add package Microsoft.EntityFrameworkCore.Design` | Necesario para generar migraciones. |

### Seguridad y autenticación

| Paquete | Comando | Descripción |
|---|---|---|
| `BCrypt.Net-Next` | `dotnet add package BCrypt.Net-Next` | Hash y verificación de contraseñas. |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | `dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer` | Middleware que valida tokens JWT. |
| `System.IdentityModel.Tokens.Jwt` | `dotnet add package System.IdentityModel.Tokens.Jwt` | Genera y firma los tokens JWT. |

### API y documentación

| Paquete | Comando | Descripción |
|---|---|---|
| `Swashbuckle.AspNetCore` | `dotnet add package Swashbuckle.AspNetCore` | Genera Swagger UI en `/swagger`. |

### Razor Pages

| Paquete | Comando | Descripción |
|---|---|---|
| `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation` | `dotnet add package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation` | Recarga vistas `.cshtml` sin recompilar. |

---

# 🔗 TallerIntegracion (Dev 2)

### Base de datos

| Paquete | Comando | Descripción |
|---|---|---|
| `Microsoft.EntityFrameworkCore.SqlServer` | `dotnet add package Microsoft.EntityFrameworkCore.SqlServer` | EF Core para la BD de integración (caché, cola, logs). |
| `Microsoft.EntityFrameworkCore.Tools` | `dotnet add package Microsoft.EntityFrameworkCore.Tools` | Comandos de migración. |
| `Microsoft.EntityFrameworkCore.Design` | `dotnet add package Microsoft.EntityFrameworkCore.Design` | Generación de migraciones. |

### Seguridad

| Paquete | Comando | Descripción |
|---|---|---|
| `Microsoft.AspNetCore.Authentication.JwtBearer` | `dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer` | Valida tokens JWT provenientes de Web y Caja. |
| `System.IdentityModel.Tokens.Jwt` | `dotnet add package System.IdentityModel.Tokens.Jwt` | Permite leer claims del token (`userId`, `rol`, `sucursalId`). |

### Llamadas HTTP al Core

| Paquete | Comando | Descripción |
|---|---|---|
| `Microsoft.Extensions.Http` | `dotnet add package Microsoft.Extensions.Http` | Habilita `IHttpClientFactory` para llamadas HTTP seguras. |
| `Polly` | `dotnet add package Polly` | Reintentos automáticos y circuit breaker cuando el Core falla. |
| `Microsoft.Extensions.Http.Polly` | `dotnet add package Microsoft.Extensions.Http.Polly` | Integra Polly con `HttpClientFactory`. |

### Cache y tareas en segundo plano

| Paquete | Comando | Descripción |
|---|---|---|
| `Microsoft.Extensions.Caching.Memory` | `dotnet add package Microsoft.Extensions.Caching.Memory` | `IMemoryCache` para guardar catálogo en RAM. |

### API y documentación

| Paquete | Comando | Descripción |
|---|---|---|
| `Swashbuckle.AspNetCore` | `dotnet add package Swashbuckle.AspNetCore` | Swagger UI para documentar los endpoints. |

---

# 🌐 TallerWeb (Dev 3)

### Base de datos

| Paquete | Comando | Descripción |
|---|---|---|
| `Microsoft.EntityFrameworkCore.SqlServer` | `dotnet add package Microsoft.EntityFrameworkCore.SqlServer` | EF Core para BD Web (sesiones, órdenes web, pagos simulados). |
| `Microsoft.EntityFrameworkCore.Tools` | `dotnet add package Microsoft.EntityFrameworkCore.Tools` | Comandos de migración. |
| `Microsoft.EntityFrameworkCore.Design` | `dotnet add package Microsoft.EntityFrameworkCore.Design` | Generación de migraciones. |

### Autenticación en Blazor

| Paquete | Comando | Descripción |
|---|---|---|
| `Microsoft.AspNetCore.Components.Authorization` | `dotnet add package Microsoft.AspNetCore.Components.Authorization` | Permite usar `[Authorize]` en componentes Razor. |
| `Blazored.LocalStorage` | `dotnet add package Blazored.LocalStorage` | Guarda el JWT en el `localStorage` del navegador. |

### Llamadas HTTP a Integración

| Paquete | Comando | Descripción |
|---|---|---|
| `Microsoft.Extensions.Http` | `dotnet add package Microsoft.Extensions.Http` | `IHttpClientFactory` para consumir la API de Integración. |

### UI y componentes

| Paquete | Comando | Descripción |
|---|---|---|
| `MudBlazor` | `dotnet add package MudBlazor` | Librería de componentes UI para Blazor. |

---

# 💳 TallerCaja (Dev 4)

### Base de datos local

| Paquete | Comando | Descripción |
|---|---|---|
| `Microsoft.EntityFrameworkCore.Sqlite` | `dotnet add package Microsoft.EntityFrameworkCore.Sqlite` | EF Core para SQLite. La BD vive como archivo `.db`. |
| `Microsoft.EntityFrameworkCore.Tools` | `dotnet add package Microsoft.EntityFrameworkCore.Tools` | Comandos de migración. |
| `Microsoft.EntityFrameworkCore.Design` | `dotnet add package Microsoft.EntityFrameworkCore.Design` | Generación de migraciones. |

### Llamadas HTTP a Integración

| Paquete | Comando | Descripción |
|---|---|---|
| `Microsoft.Extensions.Http` | `dotnet add package Microsoft.Extensions.Http` | `HttpClient` para llamar a Integración desde WinForms. |
| `Microsoft.Extensions.DependencyInjection` | `dotnet add package Microsoft.Extensions.DependencyInjection` | Agrega inyección de dependencias a WinForms. |
| `Microsoft.Extensions.Hosting` | `dotnet add package Microsoft.Extensions.Hosting` | Permite usar `IHost`, configuración y logging como en ASP.NET Core. |

### Serialización JSON

| Paquete | Comando | Descripción |
|---|---|---|
| `System.Text.Json` | Incluido en .NET 8 | Serializa y deserializa `ApiResponse` de Integración. |

### Impresión de recibos

| Paquete | Comando | Descripción |
|---|---|---|
| `QuestPDF` | `dotnet add package QuestPDF` | Genera recibos en PDF para impresión. |

### Configuración

| Paquete | Comando | Descripción |
|---|---|---|
| `Microsoft.Extensions.Configuration.Json` | `dotnet add package Microsoft.Extensions.Configuration.Json` | Lee `appsettings.json` en WinForms. |
