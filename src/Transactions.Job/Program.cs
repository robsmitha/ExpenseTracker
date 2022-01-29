using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Threading.Tasks;
using Transactions.Application;
using Transactions.Infrastructure;
using Transactions.Job.Interfaces;
using Transactions.Job.Services;

namespace Transactions.Job
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var runtime = provider.GetRequiredService<IRuntimeService>();

            try
            {
                await runtime.StartAsync();
            }
            catch (Exception)
            {
                runtime.Stop();
                throw;
            }

            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices((_, services) => services
                .AddApplication()
                .AddInfrastructure(_.Configuration)
                .AddSingleton<IRuntimeService, RuntimeService>());
    }
}
