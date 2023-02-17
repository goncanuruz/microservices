using Core.CrossCuttingConcerns.Exceptions;
using Komut.Captech.ProductService.Application;
using Komut.Captech.ProductService.Persistence;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using Komut.Captech.ProductService.WebAPI.Extensions;
using Komut.Captech.ProductService.Application.Features.Products.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddControllers();
#region EventBus
builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp => {
    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
    var factory = new ConnectionFactory()
    {
        HostName = builder.Configuration["EventBus:HostName"],
    };
    if (!string.IsNullOrWhiteSpace(builder.Configuration["EventBus:UserName"]))
    {
        factory.UserName = builder.Configuration["EventBus:UserName"];
    }
    if (!string.IsNullOrWhiteSpace(builder.Configuration["EventBus:Password"]))
    {
        factory.UserName = builder.Configuration["EventBus:Password"];
    }
    var retryCount = 5;
    if (!string.IsNullOrWhiteSpace(builder.Configuration["EventBus:RetryCount"]))
    {
        retryCount = int.Parse(builder.Configuration["EventBus:RetryCount"]);
    }
    return new DefaultRabbitMQPersistentConnection(factory, retryCount, logger);
});



builder.Services.AddSingleton<EventBusRabbitMQProducer>();
#endregion



builder.Services.AddSingleton<EventBusProductCreateConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRabbitListener();
//app.ConfigureCustomExceptionMiddleware();

app.UseAuthorization();

app.MapControllers();


app.Run();
