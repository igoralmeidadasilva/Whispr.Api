using FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Whispr.Presentation.Web;
using Whispr.Presentation.Web.Core.Handlers.HttpClient;
using Whispr.Presentation.Web.Pages.Public.Register;
using Whispr.Presentation.Web.Services.Api;
using Whispr.Presentation.Web.Services.Api.V1.Users;
using Whispr.Presentation.Web.Services.Ui.Alert;
using Whispr.Presentation.Web.Services.Ui.Modal;
using Whispr.Presentation.Web.Services.Ui.Toast;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddTransient<GlobalErrorDelegatingHandler>();
builder.Services.AddTransient<LoggingDelegatingHandler>();

builder.Services.AddHttpClient<ApiClient>(Constants.HttpClients.WhisprApi, client =>
{
    client.BaseAddress = new Uri("https://localhost:7023/");
})
//.AddHttpMessageHandler<GlobalErrorDelegatingHandler>()
.AddHttpMessageHandler<LoggingDelegatingHandler>();

builder.Services.AddScoped<IApiClient, ApiClient>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IModalService, ModalService>();
builder.Services.AddScoped<IToastService, ToastService>();
builder.Services.AddSingleton<IValidator<CreateUserModel>, CreateUserModelValidator>();

await builder.Build().RunAsync();