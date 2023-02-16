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

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CreateProductCommand createProductCommand)
        {
            CreatedProductDto result = await Mediator.Send(createProductCommand);
            return Created("", result);

        }
    }
}
