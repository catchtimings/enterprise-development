using FoodDelivery.Generator;
using FoodDelivery.Generator.Rabbitmq.Infrastructure;
using FoodDelivery.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddGeneratorServices<RabbitMqProducer>();
builder.AddRabbitMQClient("rabbitmq");

var app = builder.Build();

app.UseGeneratorServices();
app.MapDefaultEndpoints();
app.Run();