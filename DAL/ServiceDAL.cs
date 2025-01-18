using DAL.IRepo;
using DAL.Repo;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class ServiceDAL
    {
        public static IServiceCollection serviceDescriptors(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepo,CategoryRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            return services;
        }
    }
}
