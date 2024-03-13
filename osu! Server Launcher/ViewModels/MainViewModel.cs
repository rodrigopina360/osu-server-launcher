using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using osuserverlauncher.Infrastructure;
using osuserverlauncher.Models;

namespace osuserverlauncher.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        public ObservableCollection<ServerViewModel> Servers { get; }

        private ServerViewModel m_selectedServer = null;
        public ServerViewModel SelectedServer
        {
            get => m_selectedServer;
            set => SetField(ref m_selectedServer, value);
        }

        private ServerViewModel m_connectedServer = null;
        public ServerViewModel ConnectedServer
        {
            get => m_connectedServer;
            set => m_connectedServer = value; // FUTURE: notify property changed if necessary
        }

        private bool m_isOsuRunning = false;
        public bool IsOsuRunning
        {
            get => m_isOsuRunning;
            set => SetField(ref m_isOsuRunning, value);
        }

        public MainViewModel(ObservableCollection<ServerViewModel> servers)
        {
            Servers = servers;

            if (Servers.Any())
                m_selectedServer = Servers[0];
        }
    }
}
