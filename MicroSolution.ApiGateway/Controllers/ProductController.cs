using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroSolution.Infrastructure.Command.Product;
using MicroSolution.Infrastructure.Event.Product;
using MicroSolution.Infrastructure.Queries.Product;
using System;
using System.Threading.Tasks;

namespace MicroSolution.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IBusControl _bus;
        private readonly IRequestClient<GetProductById> _requestClient;

        public ProductController(IBusControl bus,IRequestClient<GetProductById> requestClient)
        {
            _bus = bus;
            _requestClient = requestClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int productId)
        {

           var prod = new GetProductById() { ProductId = productId };
            var product = await _requestClient.GetResponse<ProductCreated>(prod);
            return Accepted(product);
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
