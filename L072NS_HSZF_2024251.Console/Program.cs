using L072NS_HSZF_2024251.Application.Interfaces;
using L072NS_HSZF_2024251.Application.Services;
using L072NS_HSZF_2024251.Console.UI;
using L072NS_HSZF_2024251.Console.UI.Interfaces;
using L072NS_HSZF_2024251.Persistence.MsSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace L072NS_HSZF_2024251.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            var app = Host.CreateDefaultBuilder()
                .ConfigureServices((hostCtx, services)=>
                {
                    //Database
                    services.AddDbContext<BusContext>();
                    //Repositories
                    services.AddSingleton<IRouteRepository, RouteRepository>();
                    services.AddSingleton<IRegionRepository, RegionRepository>();
                    //Services
                    services.AddSingleton<IRouteService, RouteService>();
                    services.AddSingleton<IRegionService, RegionService>();
                    services.AddSingleton<IFileService, FileService>();
                    services.AddSingleton<IStatisticsService, StatisticsService>();
                    //Frontend UI parts
                    services.AddSingleton<IRouteActions, RouteActions>();
                    services.AddSingleton<IRegionUI, RegionUI>();
                    services.AddSingleton<IStatisticsUI, StatisticsUI>();
                    services.AddSingleton<IFileActionUI, FileActionUI>();
                    //Frontend
                    services.AddSingleton<IMainUI, Main>();
                })
                .Build();
            
            app.Start();
            
            IServiceProvider provider = app.Services.CreateScope().ServiceProvider;
            
            provider.GetService<IMainUI>().RunMain();
        }
    }
}