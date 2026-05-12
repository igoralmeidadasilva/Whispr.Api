using Serilog;
using Whispr.Application;
using Whispr.Infrastructure;
using Whispr.Presentation.Api;
using Whispr.Presentation.Api.Core.Middlewares;
using Whispr.Presentation.Api.Hubs;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));
    builder.WebHost.UseKestrel(opt => opt.AddServerHeader = false);
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddApplication(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddPresentation(builder.Configuration);

    var app = builder.Build();
    app.UseExceptionHandler(opt => { });
    app.UseSerilogRequestLogging(); 
    app.UseHttpsRedirection();
    app.UseRateLimiter();
    app.UseCors();
    app.MapEndpoints();
    app.MapHub<ChatHub>(Whispr.Presentation.Api.Constants.Hubs.ChatUrl);
    app.UseCustomSwagger();
    app.UseCustomHealthCheck();
    app.UseCustomSecurity();
    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex.ToString());
    throw;
}
finally
{
    Log.CloseAndFlush();
}