using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3HeroesTool
{
    public class BNetDownloadStartedEventArgs : EventArgs
    {
        string _downloadType;

        public string DownloadType { get { return _downloadType; } }

        public BNetDownloadStartedEventArgs(string downloadType)
        {
            _downloadType = downloadType;
        }
    }
    public delegate void DownloadStartedEventHandler(object sender, BNetDownloadStartedEventArgs e);

    public class BNetDownloadFinishedEventArgs : EventArgs
    {
        public BNetDownloadFinishedEventArgs()
        {
        }
    }
    public delegate void DownloadFinishedEventHandler(object sender, BNetDownloadFinishedEventArgs e);
}
