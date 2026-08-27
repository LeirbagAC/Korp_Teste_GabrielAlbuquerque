using BillingService.Clients;
using BillingService.Data;
using BillingService.Exceptions;
using BillingService.Mappers;
using BillingService.Service;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;

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

// Fallback para garantir que o serviço de inventário seja configurado corretamente
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
var inventoryUrl = builder.Configuration["Services:InventoryUrl"] 
                   ?? builder.Configuration["INVENTORY_SERVICE_URL"] 
                   ?? "http://localhost:5225";

builder.Services.AddHttpClient<IInventoryClient, InventoryClient>(client =>
{
    client.BaseAddress = new Uri(inventoryUrl);
}).AddPolicyHandler(GetRetryPolicy());

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddAuthorization();

var app = builder.Build();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError() 
        .WaitAndRetryAsync(3, retryAttempt => 
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
