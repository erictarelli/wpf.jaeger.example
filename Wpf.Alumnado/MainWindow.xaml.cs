using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Tag;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Alumnado.Config;
using Wpf.Alumnado.Services;

namespace Wpf.Alumnado
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly IAlumnadoService _serviceAlumnado;
        public readonly ITracer _tracer;
        public static MainWindow AppWindow;

        public MainWindow(IAlumnadoService serviceAlumnado, ITracer tracer)
        {
            AppWindow = this;
            _serviceAlumnado = serviceAlumnado;
            _tracer = tracer;
            InitializeComponent();
        }

        private async void btn_service_alumnado_get(object sender, RoutedEventArgs e)
        {

            using (var loggerFactory = LoggerFactory.Create(builder=>builder.AddConsole()))
            {
                using (var tracer = Tracing.InitTracer("wpf-example", loggerFactory))
                {
                    //await _serviceAlumnado.getAlumnos();
                    using (var scope = tracer.BuildSpan("call-service").StartActive(true))
                    {
                        using (var scope1 = tracer.BuildSpan("format-string")
                    .WithTag(Tags.HttpMethod, "GET")
                    .WithTag(Tags.HttpUrl, $"{Constant.BaseAdressServices.URI_ALUMNADO}")
                    .StartActive(true))
                        {


                            var response = string.Empty;

                            try
                            {
                                HttpClient _client = new HttpClient();

                                response = await _client.GetStringAsync(Constant.BaseAdressServices.URI_ALUMNADO);
                            }
                            catch (Exception ex)
                            {

                                response = ex.ToString();
                            }


                            scope1.Span.Log(new Dictionary<string, object>
                            {
                                [LogFields.Event] = "string.Format",
                                ["value"] = response
                            });

                            //return response;
                        }
                    }
                }

            }
        }
    }
}
