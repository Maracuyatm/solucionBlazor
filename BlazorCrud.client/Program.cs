using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorCrud.client.Services;
using BlazorCrud.client;
using CurrieTechnologies.Razor.SweetAlert2;
using BlazorCrud.Client.Services;
using Radzen;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7157") });
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();
builder.Services.AddScoped<IMarcaService, MarcaService>();
builder.Services.AddScoped<IProcesadorService, ProcesadorService>();
builder.Services.AddScoped<ISistemaOperativoService, SistemaOperativoService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DialogService>();




builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();
