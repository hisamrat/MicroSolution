using GreenPipes;
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
using MicroSolution.Infrastructure.SqlServer;
using MicroSolution.Product.Api.Handlers;
using MicroSolution.Product.Api.Repositories;
using MicroSolution.Product.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroSolution.Product.Api
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
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepositories, ProductRepositories>();

            services.AddScoped<CreateProductHandler>();
            var rabbitmqoption = new RabbitMqOption();
            Configuration.GetSection("rabbitmq").Bind(rabbitmqoption);
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateProductHandler>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitmqoption.ConnectionString), hostconfig =>
                    {
                        hostconfig.Username(rabbitmqoption.Username);
                        hostconfig.Password(rabbitmqoption.Password);
                    });
                    cfg.ReceiveEndpoint("Create_Product", c =>
                    {
                        c.PrefetchCount = 16;
                        c.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
                        c.ConfigureConsumer<CreateProductHandler>(provider);
                    });
                }));
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1" });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API");
            });
            
        }
    }
}
