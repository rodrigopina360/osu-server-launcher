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
    public Config Config => ConfigManager.Config;

    private Server m_selectedServer = null;
    public Server SelectedServer
    {
      get => m_selectedServer;
      set => SetField(ref m_selectedServer, value);
    }

    private Server m_connectedServer = null;
    public Server ConnectedServer
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

    public MainViewModel()
    {
      if (Config.Servers.Any())
        m_selectedServer = Config.Servers[0];
    }
  }
}
