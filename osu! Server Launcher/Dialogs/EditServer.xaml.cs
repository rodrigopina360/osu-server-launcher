using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using osuserverlauncher.Infrastructure;
using osuserverlauncher.Models;
using osuserverlauncher.Utils;
using osuserverlauncher.ViewModels;

namespace osuserverlauncher.Dialogs;

public sealed partial class EditServerDialog : ContentDialog
{
  private ServerDialogViewModel ViewModel = new();

  public Server Server => ViewModel.Server;

  private string m_uneditedName = "";

  public EditServerDialog(Server server)
  {
    this.InitializeComponent();

    m_uneditedName = server.Name;
    ViewModel.Server = server;
    checkBoxAddCredentials.IsChecked = server.Credentials != null;
  }
  private void UpdateIsPrimaryButtonEnabled(object sender, TextChangedEventArgs e)
  {
    IsPrimaryButtonEnabled = Server.Name != "" && Server.Domain != ""
                          && (m_uneditedName == Server.Name || !ConfigManager.Config.Servers.Any(x => x.Name.ToLower() == Server.Name.ToLower()))
                          && (!checkBoxAddCredentials.IsChecked.Value || Server.Credentials.Username != "" && StringUtil.TrimServerDomain(Server.Credentials.PlainPassword) != "");

    ViewModel.ServerAlreadyExists = m_uneditedName != Server.Name && ConfigManager.Config.Servers.Any(x => x.Name.ToLower() == Server.Name.ToLower());
  }

  private void PasswordChanged(object sender, RoutedEventArgs e)
  {
    UpdateIsPrimaryButtonEnabled(sender, null);
  }

  private void checkBoxAddCredentials_CheckedChanged(object sender, RoutedEventArgs e)
  {
    if (checkBoxAddCredentials.IsChecked.Value && Server.Credentials == null)
      Server.Credentials = new Credentials("", "");
    else if (!checkBoxAddCredentials.IsChecked.Value)
      Server.Credentials = null;

    UpdateIsPrimaryButtonEnabled(sender, null);
  }
}
