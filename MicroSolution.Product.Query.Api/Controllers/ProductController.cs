using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroSolution.Infrastructure.Event.Product;
using MicroSolution.Product.DataProvider.Services;
using System.Threading.Tasks;

namespace MicroSolution.Product.Query.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ProductCreated> GetProduct(int productId)
        {
            var product = await _productService.GetProduct(productId);
            return product;
        }
    }
}
