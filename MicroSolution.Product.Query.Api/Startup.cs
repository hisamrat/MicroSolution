using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MicroSolution.Infrastructure.EventBus;
using MicroSolution.Infrastructure.Queries.Product;
using MicroSolution.Infrastructure.SqlServer;
using MicroSolution.Product.DataProvider.Repositories;
using MicroSolution.Product.DataProvider.Services;
using MicroSolution.Product.Query.Api.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroSolution.Product.Query.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepositories, ProductRepositories>();

            services.AddScoped<GetProductByIdHandler>();
            
            var rabbitmq = new RabbitMqOption();
            Configuration.GetSection("rabbitmq").Bind(rabbitmq);
            services.AddMassTransit(x =>
            {
                x.AddConsumer<GetProductByIdHandler>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    
                    cfg.Host(new Uri(rabbitmq.ConnectionString), hostcfg =>
                    {
                        hostcfg.Username(rabbitmq.Username);
                        hostcfg.Password(rabbitmq.Password);
                    });
                    cfg.ConfigureEndpoints(provider);
                }));

            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Get Product API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            var busControl = app.ApplicationServices.GetService<IBusControl>();

            busControl.Start();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Get Product API");
            });
        }
    }
}
