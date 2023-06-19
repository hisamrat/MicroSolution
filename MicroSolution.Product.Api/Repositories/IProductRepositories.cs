using MicroSolution.Infrastructure.Command.Product;
using MicroSolution.Infrastructure.Event.Product;
using System.Threading.Tasks;

namespace MicroSolution.Product.Api.Repositories
{
    public interface IProductRepositories
    {
        Task<ProductCreated> GetProduct(int ProductId);
        Task<ProductCreated> AddProduct(CreateProduct product);
    }
}
