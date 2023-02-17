using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Komut.Captech.ProductService.Application.Features.Products.Commands.ProductCreate;
using Komut.Captech.ProductService.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Komut.Captech.ProductService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {

        private readonly EventBusRabbitMQProducer _eventBus;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(EventBusRabbitMQProducer eventBus, ILogger<ProductsController> logger)
        {
            _eventBus = eventBus;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CreateProductCommand createProductCommand)
        {

            CreatedProductDto result = await Mediator.Send(createProductCommand);
            return Created("", result);

        }
        [HttpGet("TestEventbus")]
        public ActionResult<ProductCreateEvent> Get()
        {
            ProductCreateEvent eventMessage = new ProductCreateEvent();
            eventMessage.Name = "name4";
            try
            {
                _eventBus.Publish(EventBusConstants.ProductCreateQueue, eventMessage);
            }
            catch (Exception exception)
            {

                _logger.LogError(exception, "ERROR Publishing integration event: {EventId} from {AppName}", eventMessage.RequestId, "Sourcing");
                throw;
            }
            return Accepted(eventMessage);
        }
    }
}
