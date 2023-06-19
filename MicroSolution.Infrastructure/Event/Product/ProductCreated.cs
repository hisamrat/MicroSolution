using System;
using System.Collections.Generic;
using System.Text;

namespace MicroSolution.Infrastructure.Event.Product
{
    public class ProductCreated
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
