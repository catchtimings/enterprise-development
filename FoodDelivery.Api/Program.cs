using FoodDelivery.Application.Interfaces;
using FoodDelivery.Application.Services;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;
using FoodDelivery.Infrastructure.InMemory.Repositories;

var builder = WebApplication.CreateBuilder(args);

Console.OutputEncoding = System.Text.Encoding.UTF8;
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSingleton<DataSeeder>();

builder.Services.AddSingleton<IRepository<Client>, ClientRepository>();
builder.Services.AddSingleton<IRepository<Restaurant>, RestaurantRepository>();
builder.Services.AddSingleton<IRepository<Dish>, DishRepository>();
builder.Services.AddSingleton<IRepository<DishCategory>, DishCategoryRepository>();
builder.Services.AddSingleton<IRepository<Order>, OrderRepository>();

builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<RestaurantService>();
builder.Services.AddScoped<DishService>();
builder.Services.AddScoped<DishCategoryService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<AnalyticsService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
    foreach (var xmlFile in xmlFiles) c.IncludeXmlComments(xmlFile);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();