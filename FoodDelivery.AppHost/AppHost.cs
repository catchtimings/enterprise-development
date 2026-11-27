var builder = DistributedApplication.CreateBuilder(args);

const string dbName = "fooddeliverydb";

var foodDeliveryDb = builder.AddMySql("MySQL")
    .WithDataVolume()
    .AddDatabase(dbName);

var rabbitmq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

builder.AddProject<Projects.FoodDelivery_Api>("api")
    .WithReference(foodDeliveryDb, "fooddeliverydb")
    .WithReference(rabbitmq, "rabbitmq")
    .WaitFor(foodDeliveryDb)
    .WaitFor(rabbitmq);

builder.AddProject<Projects.FoodDelivery_Generator_Rabbitmq>("generator-rabbitmq")
    .WithReference(rabbitmq, "rabbitmq")
    .WaitFor(rabbitmq);

builder.Build().Run();