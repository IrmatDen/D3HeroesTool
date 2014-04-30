using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace D3HeroesTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public D3Data.IBNetService ActiveBNet;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Any(arg => arg == "--filesystem"))
            {
                FSBNetService serv = new FSBNetService();
                serv.RootFolder = Directory.GetCurrentDirectory() + "\\sample_jsons";
                if (!Directory.Exists(serv.RootFolder))
                    throw new Exception("--filesystem can only be used from a folder containing the sample_jsons subfolder");

                ActiveBNet = serv;
            }
        }
    }
}
