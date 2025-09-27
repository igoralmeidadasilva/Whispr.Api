using Whispr.Infrastructure;
using Whispr.Presentation.Api;
using Whispr.Presentation.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors();
app.MapHub<ChatHub>("/chathub");

app.UseCustomSwagger();

app.Run();