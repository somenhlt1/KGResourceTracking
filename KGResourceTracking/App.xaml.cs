using CommunityToolkit.Mvvm.Messaging;

using KGResourceTracking.Data;
using KGResourceTracking.KingdomAP;

using MaterialDesignThemes.Wpf;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Threading;

namespace KGResourceTracking;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    [STAThread]
    public static void Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();
        host.Start();
        using (var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        using (var context = scope.ServiceProvider.GetRequiredService<KingdomGuardAPContext>())
        {
            context.Database.Migrate();
        }

        App app = new();
        app.InitializeComponent();
        app.MainWindow = host.Services.GetRequiredService<MainWindow>();
        app.MainWindow.Visibility = Visibility.Visible;
        app.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder)
            => configurationBuilder.AddUserSecrets(typeof(App).Assembly))
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<AddNewApViewModel>();
            services.AddSingleton<ViewPlayerApViewModel>();
            services.AddSingleton<WeakReferenceMessenger>();
            services.AddSingleton<IMessenger, WeakReferenceMessenger>(provider => provider.GetRequiredService<WeakReferenceMessenger>());

            services.AddSingleton(_ => Current.Dispatcher);

            services.AddDbContext<KingdomGuardAPContext>();
            services.AddSingleton<KingdomGuardPlayerDataServiceControllers>();

            services.AddTransient<ISnackbarMessageQueue>(provider =>
            {
                Dispatcher dispatcher = provider.GetRequiredService<Dispatcher>();
                return new SnackbarMessageQueue(TimeSpan.FromSeconds(3.0), dispatcher);
            });
        });
}
