using Jaeger;
using Jaeger.Samplers;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing;
using OpenTracing.Util;
using System.Net.Http;
using System.Windows;
using Wpf.Alumnado.Config;
using Wpf.Alumnado.Services;

namespace Wpf.Alumnado
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<ILogBase>(provider => new LogBase(new FileInfo(@"c:\temp\log.txt")));
            services.AddSingleton<MainWindow>();
            services.AddSingleton<IAlumnadoService, AlumandoService>();

            // Use "OpenTracing.Contrib.NetCore" to automatically generate spans for ASP.NET Core, Entity Framework Core, ...
            // See https://github.com/opentracing-contrib/csharp-netcore for details.
            services.AddOpenTracing();

            // Adds the Jaeger Tracer.
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                string serviceName = "wpf-example";

                // This will log to a default localhost installation of Jaeger.
                var tracer = new Tracer.Builder(serviceName)
                    .WithSampler(new ConstSampler(true))
                    .Build();

                // Allows code that can't use DI to also access the tracer.
                GlobalTracer.Register(tracer);

                return tracer;
            });
        }

        private void AppOnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
