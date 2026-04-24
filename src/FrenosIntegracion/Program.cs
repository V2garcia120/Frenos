using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FrenosIntegracion.Data;
using FrenosIntegracion.Middleware;
using FrenosIntegracion.Services.Cache;
using FrenosIntegracion.Services.Core;
using FrenosIntegracion.Services.Sync;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Base de Datos (SQL Server)
builder.Services.AddDbContext<IntegracionDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Integracion")));

// 2. Configurar HttpClient para el CoreService
builder.Services.AddHttpClient<ICoreService, CoreService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Core:BaseUrl"] ?? "https://localhost:7001");
    client.Timeout = TimeSpan.FromSeconds(10);
});

// 3. Configurar Seguridad (JWT)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2),
            RoleClaimType = "Rol"
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("https://localhost:7207") // Puerto de FrenosWeb
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(null);  // ← permite acceso anónimo por defecto

// 4. Registro de Servicios propios e Inyecci�n de Dependencias
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IColaSyncService, ColaSyncService>();

// 5. Registro del Servicio en segundo plano (Worker)
builder.Services.AddHostedService<SyncHostedService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

// 6. Migraciones autom�ticas al iniciar (Crea las tablas si no existen)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IntegracionDbContext>();
    await db.Database.MigrateAsync();
}

// 7. Configuraci�n del Pipeline (EL ORDEN IMPORTA)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();
}

// El Middleware de Log debe ir ANTES de Auth para captar intentos fallidos
app.UseMiddleware<RequestLogMiddleware>();

//app.UseHttpsRedirection();

app.UseCors("AllowBlazor");

app.UseAuthentication(); // Debe ir antes de Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();