using Whispr.Infrastructure;
using Whispr.Presentation.Api;
using Whispr.Presentation.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseCors();
app.MapHub<ChatHub>(Constants.Hubs.ChatUrl);
app.UseCustomSwagger();
app.UserCustomHealthCheck();

app.Run();