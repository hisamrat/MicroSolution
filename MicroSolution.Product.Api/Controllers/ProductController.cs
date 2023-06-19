using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MicroSolution.Infrastructure.Command.Product;
using MicroSolution.Product.Api.Services;
using System.Threading.Tasks;

namespace MicroSolution.Product.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetProduct(int productid)
        {
            var data= await _productService.GetProduct(productid);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(CreateProduct product)
        {
            var data = await _productService.AddProduct(product);
            return Ok(data);
        }

    }
}
