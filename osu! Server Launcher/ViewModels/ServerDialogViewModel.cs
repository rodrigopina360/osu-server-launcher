using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osuserverlauncher.Infrastructure;
using osuserverlauncher.Models;

namespace osuserverlauncher.ViewModels;
public class ServerDialogViewModel : PropertyChangedBase
{
  private Server m_server = new Server("", "");
  public Server Server
  {
    get => m_server;
    set => SetField(ref m_server, value);
  }

  private bool m_serverAlreadyExists = false;
  public bool ServerAlreadyExists
  {
    get => m_serverAlreadyExists;
    set => SetField(ref m_serverAlreadyExists, value);
  }
}
