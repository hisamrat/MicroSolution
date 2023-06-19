using Microsoft.EntityFrameworkCore;
using MicroSolution.Infrastructure.Command.Product;
using MicroSolution.Infrastructure.Event.Product;
using MicroSolution.Infrastructure.SqlServer;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MicroSolution.Product.Api.Repositories
{
    public class ProductRepositories : IProductRepositories
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepositories(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ProductCreated> AddProduct(CreateProduct product)
        {
           await _dbContext.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return new ProductCreated { ProductId=product.ProductId,ProductName=product.ProductName,CreatedAt=DateTime.Now };
        }

        public async Task<ProductCreated> GetProduct(int ProductId)
        {
            var data =await _dbContext.Products.Where(c => c.ProductId == ProductId).FirstOrDefaultAsync();
            return new ProductCreated { ProductId = data.ProductId, ProductName = data.ProductName};
        }
    }
}
