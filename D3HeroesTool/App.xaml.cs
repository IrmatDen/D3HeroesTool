using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace D3HeroesTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App Instance;
        public static string AppDirectory;
        public event EventHandler OnLanguageChanged;

        public static D3Data.IBNetService ActiveBNet;

        public App()
        {
            Instance = this;
            AppDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            // Load the Localization Resource Dictionary based on OS language
            SetLanguageResourceDictionary(GetLocXAMLFilePath(CultureInfo.CurrentCulture.Name));
        }
        
        /// <summary>
        /// Dynamically load a Localization ResourceDictionary from a file
        /// </summary>
        public void SwitchLanguage(string inFiveCharLang)
        {
            if (CultureInfo.CurrentCulture.Name.Equals(inFiveCharLang))
                return;

            CultureInfo ci = null;
            try
            {
                ci = new CultureInfo(inFiveCharLang);
            }
            catch (CultureNotFoundException)
            {
                ci = new CultureInfo("en-US");
            }
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            SetLanguageResourceDictionary(GetLocXAMLFilePath(inFiveCharLang));
            if (null != OnLanguageChanged)
            {
                OnLanguageChanged(this, new EventArgs());
            }
        }

        private void SetLanguageResourceDictionary(string inFile)
        {
            if (File.Exists(inFile))
            {
                // Read in ResourceDictionary File
                var languageDictionary = new ResourceDictionary();
                languageDictionary.Source = new Uri(inFile);

                // Remove any previous Localization dictionaries loaded
                int langDictId = -1;
                for (int i = 0; i < Resources.MergedDictionaries.Count; i++)
                {
                    var md = Resources.MergedDictionaries[i];
                    // Make sure your Localization ResourceDictionarys have the ResourceDictionaryName
                    // key and that it is set to a value starting with "Loc-".
                    if (md.Contains("StringResourcesDictName"))
                    {
                        if (md["StringResourcesDictName"].ToString().StartsWith("Str-"))
                        {
                            langDictId = i;
                            break;
                        }
                    }
                }
                if (langDictId == -1)
                {
                    // Add in newly loaded Resource Dictionary
                    Resources.MergedDictionaries.Add(languageDictionary);
                }
                else
                {
                    // Replace the current langage dictionary with the new one
                    Resources.MergedDictionaries[langDictId] = languageDictionary;
                }
            }
        }

        private string GetLocXAMLFilePath(string inFiveCharLang)
        {
            string locXamlFile = "StringResources." + inFiveCharLang + ".xaml";
            return Path.Combine(AppDirectory, inFiveCharLang, locXamlFile);
        }

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
