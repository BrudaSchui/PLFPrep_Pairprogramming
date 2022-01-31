using ChinookDbLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace PLFPrep
{
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = Host.CreateDefaultBuilder()
              .ConfigureServices((ctx, services) => ConfigureServices(ctx.Configuration, services))
              .Build();
        }


        internal void ConfigureServices(IConfiguration config, IServiceCollection services)
        {
            string connectionString = config.GetConnectionString("ChinookDb");
            services.AddDbContext<ChinookContext>(x => x.UseSqlite(connectionString));
            services.AddSingleton<MainWindow>();
            services.AddSingleton<ChinookViewModel>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }
            base.OnExit(e);
        }


    }
}
