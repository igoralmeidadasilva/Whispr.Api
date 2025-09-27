using Serilog;
using Whispr.Application;
using Whispr.Infrastructure;
using Whispr.Presentation.Api;
using Whispr.Presentation.Api.Hubs;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddAppication(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddPresentation(builder.Configuration);

    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseRateLimiter();
    app.UseCors();
    app.MapHub<ChatHub>(Constants.Hubs.ChatUrl);
    app.UseCustomSwagger();
    app.UserCustomHealthCheck();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex.ToString());
    throw;
}
finally
{
    Log.CloseAndFlush();
}