var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "guest");
var password = builder.AddParameter("password", "guest");
var rabbitmq = builder.AddRabbitMQ("messaging", username, password)
    .WithManagementPlugin();

builder.AddProject<Projects.Order_Api>("order-api")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq);

builder.AddProject<Projects.Shipping>("shipping")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq);

builder.Build().Run();
