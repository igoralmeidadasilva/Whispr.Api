using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Whispr.Presentation.Web;
using Whispr.Presentation.Web.Services.Alert;
using Whispr.Presentation.Web.Services.Modal;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IModalService, ModalService>();

await builder.Build().RunAsync();