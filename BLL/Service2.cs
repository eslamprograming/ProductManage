using BLL.IService;
using BLL.Service;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class Service2
    {
        public static IServiceCollection BLLServicer(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductServic, ProductService>();
            //services.AddScoped<IAccountService,AccountService>();
            return services;
        }
    }
}
