using InventoryService.Data;
using InventoryService.Exceptions;
using Microsoft.EntityFrameworkCore;
using InventoryService.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("InventoryConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A Connection String 'InventoryConnection' não foi encontrada no appsettings.json.");
}

// Configura o DbContext
builder.Services.AddDbContext<InventoryDbContext>(options =>
{
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30)));
});

builder.Services.AddScoped<IProductService, ProductService>();

// Configura o comportamento da API para lidar com erros de validação de modelo, mudar caso seja necessário para atender melhor as necessidades do front
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value is not null && e.Value.Errors.Count > 0)
                .Select(e => new 
                {
                    Campo = e.Key,
                    Mensagens = e.Value!.Errors.Select(x => x.ErrorMessage).ToArray()
                })
                .ToArray();

            var problemDetails = new ValidationProblemDetails
            {
                Title = "Erro de Validação nos Dados Enviados",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Um ou mais campos da requisição estão inválidos. Verifique a propriedade 'errors'.",
                Instance = context.HttpContext.Request.Path
            };

            problemDetails.Extensions.Add("errors", errors);

            return new BadRequestObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json" }
            };
        };
    });

// Configura o manipulador global de exceções
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
