var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient("inventoryClient",
    client => client.BaseAddress = new(builder.Configuration["Services:inventory:http:0"]));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/stock", async (IHttpClientFactory clientFactory, CancellationToken cancellationToken) =>
{
    var inventoryClient = clientFactory.CreateClient("inventoryClient");

    return await inventoryClient.GetFromJsonAsync<dynamic>($"/api/inventory?page=1&pageSize=1000000", cancellationToken);
})
.WithName("GetStock");

app.Run();
