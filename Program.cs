using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataProcessor.Configuration;
using DataProcessor.Models;
using DataProcessor.Services;

namespace DataProcessor
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            try
            {
                var dataProcessorService = host.Services.GetRequiredService<IDataProcessorService>();
                await dataProcessorService.ProcessUnprocessedRecordsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 1;
            }
            
            return 0;
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;
                    
                    services.Configure<AppSettings>(configuration);
                    
                    var appSettings = new AppSettings();
                    configuration.Bind(appSettings);
                    
                    services.AddSingleton(appSettings.ApiSettings);
                    services.AddSingleton(appSettings.DatabaseSettings);
                    services.AddSingleton(appSettings.RetrySettings);
                    
                    services.AddDbContext<DataContext>(options =>
                    {
                        options.UseOracle(appSettings.DatabaseSettings.ConnectionString);
                    });
                    
                    services.AddScoped<IApiService, ApiService>();
                    services.AddScoped<IDataProcessorService, DataProcessorService>();
                });
    }
}
