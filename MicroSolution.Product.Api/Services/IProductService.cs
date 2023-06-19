﻿using MicroSolution.Infrastructure.Command.Product;
using MicroSolution.Infrastructure.Event.Product;
using System.Threading.Tasks;

namespace MicroSolution.Product.Api.Services
{
    public interface IProductService
    {
        Task<ProductCreated> AddProduct(CreateProduct product);
        Task<ProductCreated> GetProduct(int ProductId);
    }
}
