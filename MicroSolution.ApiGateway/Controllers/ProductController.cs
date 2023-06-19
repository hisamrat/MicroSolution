using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroSolution.Infrastructure.Command.Product;
using System;
using System.Threading.Tasks;

namespace MicroSolution.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IBusControl _bus;

        public ProductController(IBusControl bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
            await Task.CompletedTask;
            return Accepted();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateProduct product)
        {
            var uri = new Uri("rabbitmq://localhost/Create_Product");
            var endpoint = await _bus.GetSendEndpoint(uri);
            await endpoint.Send(product);

            return Accepted("Product created");
        }
    }
}
