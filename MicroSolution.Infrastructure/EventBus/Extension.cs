using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitmq.ConnectionString), hostcfg =>
                    {
                        hostcfg.Username(rabbitmq.Username);
                        hostcfg.Password(rabbitmq.Password);
                    });
                }));

            });
            return services;
        }
    }
}
