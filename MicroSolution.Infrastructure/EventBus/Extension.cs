using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroSolution.Infrastructure.Queries.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroSolution.Infrastructure.EventBus
{
    public static class Extension
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services,IConfiguration configuration)
        {
            var rabbitmq = new RabbitMqOption();
            configuration.GetSection("rabbitmq").Bind(rabbitmq);

            //establish connection with rabbitmq

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitmq.ConnectionString), hostcfg =>
                    {
                        hostcfg.Username(rabbitmq.Username);
                        hostcfg.Password(rabbitmq.Password);
                    });
                    cfg.ConfigureEndpoints(provider);
                }));
                x.AddRequestClient<GetProductById>();
               
            });
            services.AddMassTransitHostedService();
            return services;
        }
    }
}
