using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using Komut.Captech.ProductService.Application.Features.Products.Commands.ProductCreate;
using Komut.Captech.ProductService.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Komut.Captech.ProductService.Application.Features.Products.Consumers
{
    public class EventBusProductCreateConsumer
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public EventBusProductCreateConsumer(IRabbitMQPersistentConnection persistentConnection, IMapper mapper, IServiceScopeFactory serviceScopeFactory)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Consume()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstants.ProductCreateQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ReceivedEvent;

            channel.BasicConsume(queue: EventBusConstants.ProductCreateQueue, autoAck: true, consumer: consumer);
        }

        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.Span);
            var @event = JsonConvert.DeserializeObject<ProductCreateEvent>(message);

            if (e.RoutingKey == EventBusConstants.ProductCreateQueue)
            {
                var command = _mapper.Map<CreateProductCommand>(@event);
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await _mediator.Send(command);
                }
            }
         
        }

        public void Disconnect()
        {
            _persistentConnection.Dispose();
        }
    }
}
