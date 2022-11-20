using Api.Services;
using CleanArch.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddScoped<IApplicationService, AppServiceHomebrewed>();
builder.Services.AddScoped<IUseCaseHandlerMarker, PingUseCaseHandler>();

var app = builder.Build();

app.MapGrpcService<DoomsdayClockService>();
app.MapGrpcReflectionService();

app.Run();
