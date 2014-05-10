﻿using System;
using System.IO;
using System.Windows;

namespace D3HeroesTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Used to access battle.net data. If data isn't available or is outdated,
        /// it'll be requested to a specified internet provider
        /// </summary>
        public static FSBNetService FSProvider;

        /// <summary>
        /// Accessible to handle download events. Shouldn't be used for anything else, use FSProvider instead.
        /// </summary>
        public static WebBNetService WebProvider;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            QuickConverter.EquationTokenizer.AddNamespace(typeof(object));

            WebProvider = new WebBNetService();
            FSProvider = new FSBNetService(WebProvider);
            FSProvider.RootFolder = Path.Combine(Directory.GetCurrentDirectory(), "cache");
            Directory.CreateDirectory(FSProvider.RootFolder);
            if (!Directory.Exists(FSProvider.RootFolder))
                throw new Exception("Application cannot be used in a folder with readonly access.");
        }
    }
}
