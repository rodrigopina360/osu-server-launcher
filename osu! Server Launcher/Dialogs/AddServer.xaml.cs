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
using System.Collections.ObjectModel;

namespace osuserverlauncher.Dialogs;
public sealed partial class AddServerDialog : ContentDialog
{
  public ServerDialogViewModel ViewModel { get; } = new();

  private ObservableCollection<ServerViewModel> m_servers = null;
  private Credentials m_cachedCredentials = null;

  public AddServerDialog(ObservableCollection<ServerViewModel> servers)
  {
    this.InitializeComponent();
    m_servers = servers;
  }

  private void UpdateIsPrimaryButtonEnabled(object sender, TextChangedEventArgs e)
  {
    ViewModel.ServerAlreadyExists = m_servers.Any(x => x.Name.ToLower() == ViewModel.Server.Name.ToLower());

    IsPrimaryButtonEnabled = ViewModel.Server.Name != "" && ViewModel.Server.Domain != ""
                          && !ViewModel.ServerAlreadyExists
                          && (!checkBoxAddCredentials.IsChecked.Value || ViewModel.Server.Credentials.Username != "" && StringUtil.TrimServerDomain(ViewModel.Server.Credentials.PlainPassword) != "");

  }

  private void PasswordChanged(object sender, RoutedEventArgs e)
  {
    UpdateIsPrimaryButtonEnabled(sender, null);
  }

  private void checkBoxAddCredentials_CheckedChanged(object sender, RoutedEventArgs e)
  {
    if (checkBoxAddCredentials.IsChecked.Value)
      ViewModel.Server.Credentials = m_cachedCredentials ?? new Credentials("", "");
    else
    {
      m_cachedCredentials = ViewModel.Server.Credentials;
      ViewModel.Server.Credentials = null;
    }

    UpdateIsPrimaryButtonEnabled(sender, null);
  }
}
