using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stats.ViewModels;
using Stats.Views.Windows;
using Stats.Abstractions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Stats.Infrastructure.Services;

namespace Stats
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        }
        public IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = new HostBuilder();
            hostBuilder.UseContentRoot(Environment.CurrentDirectory)
                .ConfigureAppConfiguration((host, config) =>
                {
                    config.SetBasePath(Environment.CurrentDirectory);
                    config.AddJsonFile("appsettings.json", true, true);
                }).ConfigureServices(ConfigureServices);
            return hostBuilder;
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICovidDataService, CovidDataService>();
            serviceCollection.AddSingleton<ICovidExcelService, CovidExcelService>();

            serviceCollection.AddSingleton<MenuWindowViewModel>();
            serviceCollection.AddSingleton<CovidStatsViewModel>();

            serviceCollection.AddSingleton<MenuWindow>();
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync().ConfigureAwait(false);
            base.OnStartup(e);
            var window = _host.Services.GetRequiredService<MenuWindow>();
            window.Show();
        }
        protected async override void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            base.OnExit(e);
            _host.Dispose();
        }
    }
}
