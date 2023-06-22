using MassTransit;
using MicroSolution.Infrastructure.Event.Product;
using MicroSolution.Infrastructure.Queries.Product;
using MicroSolution.Product.DataProvider.Services;
using System;
using System.Threading.Tasks;

namespace MicroSolution.Product.Query.Api.Handlers
{
    public class GetProductByIdHandler : IConsumer<GetProductById>
    {
        private readonly IProductService _productService;

        public GetProductByIdHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task Consume(ConsumeContext<GetProductById> context)
        {
            var product =await _productService.GetProduct(context.Message.ProductId);
            await context.RespondAsync<ProductCreated>(product);
        }
    }
}
