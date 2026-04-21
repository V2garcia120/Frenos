using FrenosWeb;
using FrenosWeb.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<FrenosWeb.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7223/")
});

builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ServicioService>();
builder.Services.AddScoped<CarritoStateService>();
builder.Services.AddScoped<OrdenWebService>();
builder.Services.AddScoped<PagoService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<VehiculoService>();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, TestAuthStateProvider>();

await builder.Build().RunAsync();