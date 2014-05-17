using System.Net;
using System.Windows;

namespace D3HeroesTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gateway for Battle.Net requests
        /// </summary>
        public static WebBNetService BNetService;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            WebRequest.DefaultCachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.Default);

#if FIDDLER
            WebRequest.DefaultWebProxy = new WebProxy("127.0.0.1", 8888);
#endif

            QuickConverter.EquationTokenizer.AddNamespace(typeof(object));
            QuickConverter.EquationTokenizer.AddNamespace(typeof(System.Windows.Visibility));

            BNetService = new WebBNetService();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            BNetService.Dispose();
        }
    }
}
