using System.Reflection;
using FluentValidation;
using Inventory.Database;
using Inventory.Extensions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var assembly = Assembly.GetExecutingAssembly();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.AddNpgsqlDbContext<InventoryContext>("inventoryDb");

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(assembly));
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddEndpoints(assembly);
builder.Services.AddProblemDetails();
builder.Services.AddCors();

var app = builder.Build();

app.MapEndpoints();

await MigrateUp();

app.UseCors(c => c
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();

async Task MigrateUp()
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<InventoryContext>();

    await context.Database.MigrateAsync();
}