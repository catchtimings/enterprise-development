var builder = DistributedApplication.CreateBuilder(args);

const string dbName = "fooddeliverydb";

var foodDeliveryDb = builder.AddMySql("MySQL")
    .WithDataVolume()
    .AddDatabase(dbName);

builder.AddProject<Projects.FoodDelivery_Api>("api")
    .WithReference(foodDeliveryDb, "fooddeliverydb")
    .WaitFor(foodDeliveryDb);

builder.Build().Run();
