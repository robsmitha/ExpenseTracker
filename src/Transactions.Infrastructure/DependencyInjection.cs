using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Transactions.Application.Interfaces;
using Transactions.Infrastructure.Context;
using Transactions.Infrastructure.Services;
using Transactions.Infrastructure.Settings;

namespace Transactions.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
                   options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IApplicationContext>(provider => provider.GetService<ApplicationContext>());

            services.Configure<PlaidSettings>(configuration.GetSection(nameof(PlaidSettings)));
            services.Configure<EPPlusSettings>(configuration.GetSection(nameof(EPPlusSettings)));
            services.AddTransient<IFinancialService, PlaidService>();
            services.AddTransient<IExcelService, EPPlusService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddHttpClient();
            return services;
        }

    }
}
