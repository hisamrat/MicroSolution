using MicroSolution.Infrastructure.Command.Product;
using MicroSolution.Infrastructure.Event.Product;
using MicroSolution.Product.Api.Repositories;
using System.Threading.Tasks;

namespace MicroSolution.Product.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepositories _productRepositories;

        public ProductService(IProductRepositories productRepositories)
        {
            _productRepositories = productRepositories;
        }
        public Task<ProductCreated> AddProduct(CreateProduct product)
        {
            return _productRepositories.AddProduct(product);
        }

        public async Task<ProductCreated> GetProduct(int ProductId)
        {
            return await _productRepositories.GetProduct(ProductId); ;
        }
    }
}
