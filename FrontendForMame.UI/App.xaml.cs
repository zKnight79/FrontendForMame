using FrontendForMame.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Reflection;
using System.Windows;

namespace FrontendForMame.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;
    private readonly IServiceProvider _serviceProvider;

    protected ILogger<App> Logger { get; private set; }

    public App()
    {
        IHostBuilder hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.UseSerilog((hostingContext, loggerConfiguration)
            => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
        hostBuilder.ConfigureServices(services =>
        {
            services.AddTransient<IMameService, MameService>();
            services.AddTransient<MainWindow>();
        });
        _host = hostBuilder.Build();

        IServiceScope scope = _host.Services.CreateScope();
        _serviceProvider = scope.ServiceProvider;

        Logger = GetService<ILogger<App>>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        Logger.LogInformation("Application is starting ...");

        try
        {
            Window mainWindow = GetService<MainWindow>();
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Unable to launch main window ({nameof(MainWindow)}) !");
            Shutdown();
        }

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        Logger.LogInformation("Application is shutting down ...");

        using (_host)
        {
            await _host.StopAsync();
        }

        base.OnExit(e);
    }

    private TService GetService<TService>() where TService : notnull
    {
        return _serviceProvider.GetRequiredService<TService>();
    }

    public static TService? GetAppService<TService>() where TService : notnull
    {
        TService? serviceInstance = default;

        if (Current is App app)
        {
            serviceInstance = app.GetService<TService>();
        }

        return serviceInstance;
    }

    public static string Version
    {
        get
        {
            string version = "";

            Assembly? ass = Assembly.GetEntryAssembly();
            if (ass is not null)
            {
                AssemblyInformationalVersionAttribute? attr = ass.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                if (attr is not null)
                {
                    version = attr.InformationalVersion;
                }
            }

            return version;
        }
    }
}
