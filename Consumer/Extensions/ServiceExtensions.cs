using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Consumer.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureBus(this IServiceCollection services)
        {
            var configurationManager = services.BuildServiceProvider().GetService<MessageContract.Models.ConfigurationManager>();
            var httpClient = services.BuildServiceProvider().GetService<IHttpClientFactory>().CreateClient();
            httpClient.BaseAddress = new Uri(configurationManager.OrderEventApi);
            httpClient.Timeout = TimeSpan.FromMinutes(10);

            services.AddMassTransit(x =>
            {
                x.AddBus(p => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(5);
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 10;
                        cb.ResetInterval = TimeSpan.FromMinutes(10);
                    });

                    config.Host(configurationManager.DataSource, (ushort)configurationManager.Port, "/", s =>
                    {
                        s.Username(configurationManager.UserId);
                        s.Password(configurationManager.Password);
                    });
                    config.ReceiveEndpoint("US-OrderAsync_state_yeni", ep =>
                    {

                        ep.Consumer(() => new OrderConsumer(httpClient, Log.Logger));
                    });
                    config.ReceiveEndpoint("US-Basket_state_yeni", ep =>
                    {

                        ep.Consumer(() => new BasketConsumer(httpClient, Log.Logger));
                    });
                    config.ReceiveEndpoint("US-Stock_state_yeni", ep =>
                    {

                        ep.Consumer(() => new StockConsumer(httpClient, Log.Logger));
                    });
                   
                }));
            });
        }
    }
}
