using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
public sealed partial class AddServerDialog : ContentDialog
{
  private ServerDialogViewModel ViewModel = new();
  private Config m_config = null;

  public Server Server => ViewModel.Server; 

  public bool SaveCredentials { get; private set; }

  public AddServerDialog(Config config)
  {
    this.InitializeComponent();
    m_config = config;
  }

  private void UpdateIsPrimaryButtonEnabled(object sender, TextChangedEventArgs e)
  {
    IsPrimaryButtonEnabled = Server.Name != "" && Server.Domain != ""
                          && !m_config.Servers.Any(x => x.Name.ToLower() == Server.Name.ToLower())
                          && (!checkBoxAddCredentials.IsChecked.Value || Server.Credentials.Username != "" && StringUtil.TrimServerDomain(Server.Credentials.PlainPassword) != "");

    ViewModel.ServerAlreadyExists = m_config.Servers.Any(x => x.Name.ToLower() == Server.Name.ToLower());
  }

  private void PasswordChanged(object sender, RoutedEventArgs e)
  {
    UpdateIsPrimaryButtonEnabled(sender, null);
  }

  private Credentials m_cached = null;

  private void checkBoxAddCredentials_CheckedChanged(object sender, RoutedEventArgs e)
  {
    if (checkBoxAddCredentials.IsChecked.Value)
      Server.Credentials = m_cached ?? new Credentials("", "");
    else
    {
      m_cached = Server.Credentials;
      Server.Credentials = null;
    }

    UpdateIsPrimaryButtonEnabled(sender, null);
  }
}
