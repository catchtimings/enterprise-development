using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Application.Interfaces.Services;
using FoodDelivery.Application.Services;
using FoodDelivery.Infrastructure.EFCore;
using FoodDelivery.Infrastructure.EFCore.Repositories;
using FoodDelivery.ServiceDefaults;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Console.OutputEncoding = System.Text.Encoding.UTF8;

builder.AddMySqlDbContext<FoodDeliveryDbContext>("fooddeliverydb");

builder.AddServiceDefaults();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<IClientRepository, ClientEfCoreRepository>();
builder.Services.AddScoped<IRestaurantRepository, RestaurantEfCoreRepository>();
builder.Services.AddScoped<IDishRepository, DishEfCoreRepository>();
builder.Services.AddScoped<IDishCategoryRepository, DishCategoryEfCoreRepository>();
builder.Services.AddScoped<IOrderRepository, OrderEfCoreRepository>();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IDishCategoryService, DishCategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<FoodDeliveryDbContext>();
    db.Database.Migrate();
}

app.MapDefaultEndpoints();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
