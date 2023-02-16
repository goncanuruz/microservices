using Komut.Captech.ProductService.Application.Services.Repositories;
using Komut.Captech.ProductService.Persistence.Contexts;
using Komut.Captech.ProductService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Komut.Captech.ProductService.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
                                                                IConfiguration configuration)
        {
            services.AddDbContext<BaseDbContext>(options =>
                                                     options.UseSqlServer(
                                                         configuration.GetConnectionString("ProductServiceConnectionString")));
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}