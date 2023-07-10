using BasketApi.Services.Contracts;
using BasketApi.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add IService dependencies through DI
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IProductService, ProductService>();

// Added as Singleton to be reused accross requests
builder.Services.AddSingleton<HttpClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", async (HttpContext context) =>
{
    await context.Response.WriteAsync("BasketAPI Entry Point");
});

app.Run();

// As a minimal API approach is used, Program is inaccessible for Integration testing
public partial class Program { }