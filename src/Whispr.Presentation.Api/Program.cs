using Whispr.Presentation.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7059", "http://localhost:5223")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}
app.UseCors();

app.UseHttpsRedirection();
app.MapHub<ChatHub>("/chathub");

app.Run();