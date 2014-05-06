using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace D3HeroesTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static D3Data.IBNetService ActiveBNet;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Any(arg => arg == "--filesystem"))
            {
                FSBNetService serv = new FSBNetService();
                serv.RootFolder = Path.Combine(Directory.GetCurrentDirectory(), "cache");
                if (!Directory.Exists(serv.RootFolder))
                    throw new Exception("--filesystem can only be used from a folder containing the cache subfolder");

                ActiveBNet = serv;
            }
        }
    }
}
