using BillingService.Clients;
using BillingService.Data;
using BillingService.Mappers;
using BillingService.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("InvoiceConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A Connection String 'InvoiceConnection' não foi encontrada no appsettings.json.");
}

// Configura o DbContext
builder.Services.AddDbContext<BillingDbContext>(options =>
{
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30)));
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<InvoiceMapper>();

builder.Services.AddScoped<IInvoiceService, InvoiceService>();
var inventoryUrl = builder.Configuration["Services:InventoryUrl"] 
                   ?? builder.Configuration["INVENTORY_SERVICE_URL"] 
                   ?? "http://localhost:5225";
builder.Services.AddHttpClient<IInventoryClient, InventoryClient>(client =>
{
    client.BaseAddress = new Uri(inventoryUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
