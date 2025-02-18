using System.Reflection;
using FluentValidation;
using Inventory.Database;
using Inventory.Extensions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var assembly = Assembly.GetExecutingAssembly();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<InventoryContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("inventoryDb")));

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(assembly));
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddEndpoints(assembly);
builder.Services.AddCors();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<InventoryContext>();

    await context.Database.MigrateAsync();
}

app.UseCors(c => c
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();