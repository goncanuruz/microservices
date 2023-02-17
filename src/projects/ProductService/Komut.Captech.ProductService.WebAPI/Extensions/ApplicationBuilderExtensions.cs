using Komut.Captech.ProductService.Application.Features.Products.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;

namespace Komut.Captech.ProductService.WebAPI.Extensions
{

    public static class ApplicationBuilderExtensions
    {
        public static EventBusProductCreateConsumer Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<EventBusProductCreateConsumer>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);
            return app;
        }

        private static void OnStopping()
        {
            Listener.Consume();
        }

        private static void OnStarted()
        {
            Listener.Disconnect();
        }
    }
}