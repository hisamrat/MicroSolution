using Microsoft.EntityFrameworkCore;
using MicroSolution.Infrastructure.Command.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroSolution.Infrastructure.SqlServer
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<CreateProduct> Products { get; set; }
    }
}
