using MassTransit;
using MicroSolution.Infrastructure.Command.Product;

using MicroSolution.Product.DataProvider.Services;
using System.Threading.Tasks;

namespace MicroSolution.Product.Api.Handlers
{
    public class CreateProductHandler : IConsumer<CreateProduct>
    {
        private readonly IProductService _productService;

        public CreateProductHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task Consume(ConsumeContext<CreateProduct> context)
        {
            await _productService.AddProduct(context.Message);
            await Task.CompletedTask;
        }
    }
}
