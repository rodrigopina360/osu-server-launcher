using Microsoft.UI;
using Microsoft.UI.Windowing;
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
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using WinRT.Interop;
using osuserverlauncher.Dialogs;
using osuserverlauncher.Infrastructure;
using osuserverlauncher.Models;
using osuserverlauncher.Services;
using osuserverlauncher.Utils;
using osuserverlauncher.ViewModels;
using DiscordRPC;

// To learn more about osuserverlauncher, the osuserverlauncher project structure,
// and more about our project templates, see: http://aka.ms/osuserverlauncher-project-info.

namespace osuserverlauncher
{
  /// <summary>
  /// An empty window that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainWindow : Window
  {
    private MainViewModel ViewModel = null;
    private ConfigManager m_configManager = null;

    public MainWindow(ConfigManager configManager)
    {
      this.InitializeComponent();

      ViewModel = new MainViewModel(configManager.Config);
      m_configManager = configManager;

#if DEBUG
      AppWindow appWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(this)));
      appWindow.Resize(new SizeInt32(650, 500));
#endif

      OsuWatcher.Start(ViewModel, new DiscordRPCManager(new DiscordRpcClient("770355757622755379", -1)));

      ExtendsContentIntoTitleBar = true;
      SetTitleBar(titleBar);

      Title = $"osu! Server Launcher {Assembly.GetExecutingAssembly().GetName().Version}";
    }

    private void ShowInfo(InfoBarSeverity severity, string title, string message)
    {
      infoBar.Severity = severity;
      infoBar.Title = title;
      infoBar.Message = message;
      infoBar.IsOpen = true;
    }

    #region Other UI events

    private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // TODO: put in xaml, need to make that selectedserver = null makes bindings turn to blank texts
      if (ViewModel.SelectedServer == null)
      {
        textBlockServerName.Text = "";
        textBlockServerDomain.Text = "";
        textBlockDescription.Text = "";
        textBlockLastPlayed.Text = "";
        imageLogo.Source = null;
        return;
      }

      // TODO: put in xaml
      textBlockLastPlayed.Text = ViewModel.SelectedServer.LastPlayed != DateTime.MinValue ?
                                 $"Last played on {ViewModel.SelectedServer.LastPlayed.ToShortDateString()}" :
                                 "Never played";

      // TODO: can this be put in xaml?
      textBlockDescription.FontStyle = FontStyle.Italic;
      textBlockDescription.Foreground = new SolidColorBrush(Colors.Gray);
      textBlockDescription.Text = "Loading..."; // clear description textblock before getting manifest
                                                // to not show the currently displayed one when switching
      OSLManifest manifest = await ViewModel.SelectedServer.GetManifest();

      if (manifest.Description == "")
      {
        textBlockDescription.FontStyle = FontStyle.Italic;
        textBlockDescription.Foreground = new SolidColorBrush(Colors.Gray);
        textBlockDescription.Text = "This server provides no description.";
      }
      else
      {
        ResourceDictionary theme = (ResourceDictionary) Application.Current.Resources.ThemeDictionaries[App.Current.RequestedTheme.ToString()];
        textBlockDescription.FontStyle = FontStyle.Normal;
        textBlockDescription.Foreground = (SolidColorBrush) theme["TextForeground"];
        textBlockDescription.Text = manifest.Description;
      }
    }

    private async void LaunchOsu_Click(object sender, RoutedEventArgs e)
    {
      string osuPath = OsuUtil.GetOsuPath();

      if (osuPath == null)
      {
        ShowInfo(InfoBarSeverity.Error, $"osu! installation not found", "Your osu! installation could not be found. Please make sure you ran osu! as administrator at least once. If this issue persists, please visit our Discord.");
        return;
      }

      string osuConfigFile = Path.Combine(osuPath, $"osu!.{Environment.UserName}.cfg");
      string osuExe = Path.Combine(osuPath, $"osu!.exe");

      if (ViewModel.SelectedServer.Credentials != null)
      {
        if (File.Exists(osuConfigFile))
        {
          OsuUtil.SetServerCredentials(osuConfigFile, ViewModel.SelectedServer);
          await Task.Delay(100); // Make sure the osu config file is saved
        }
        else
          ShowInfo(InfoBarSeverity.Error, $"osu!.{Environment.UserName}.cfg not found", "Your osu! config file could not be found, therefore you could not automatically be logged in. If this issue persists, please visit our Discord for further help.");
      }

      ProcessStartInfo psi = new ProcessStartInfo()
      {
        FileName = osuExe
      };
      if (!ViewModel.SelectedServer.IsBancho)
        psi.Arguments = $"-devserver \"{ViewModel.SelectedServer.Domain}\"";

      Process.Start(psi);

      ViewModel.ConnectedServer = ViewModel.SelectedServer;
      ViewModel.SelectedServer.LastPlayed = DateTime.Now;
      m_configManager.Save();
    }

    #endregion

    #region ListView Context Flyout

    private void OpenInWeb_Click(object sender, RoutedEventArgs e)
    {
      Server server = (sender as MenuFlyoutItem).DataContext as Server;

      Process.Start(new ProcessStartInfo()
      {
        FileName = $"https://{server.Domain}",
        UseShellExecute = true
      });
    }

    private async void EditServer_Click(object sender, RoutedEventArgs e)
    {
      Server server = (sender as MenuFlyoutItem).DataContext as Server;

      EditServerDialog cd = new EditServerDialog(server.Clone(), m_configManager.Config) // clone to only save when primary button clicked
      {
        XamlRoot = Content.XamlRoot
      };

      ContentDialogResult result = await cd.ShowAsync();
      if (result == ContentDialogResult.Primary)
      {
        int index = ViewModel.Config.Servers.IndexOf(server);
        ViewModel.Config.Servers[index] = cd.Server;
        ViewModel.SelectedServer = cd.Server;
        m_configManager.Save();
      }
    }

    private async void RemoveServer_Click(object sender, RoutedEventArgs e)
    {
      Server server = (sender as MenuFlyoutItem).DataContext as Server;

      ContentDialog cd = new ContentDialog()
      {
        Title = $"Do you want to remove '{server.Name}'?",
        Content = "This includes credentials you may have saved. This action cannot be undone.",
        PrimaryButtonText = "Remove",
        CloseButtonText = "Cancel",
        DefaultButton = ContentDialogButton.Primary,
        XamlRoot = Content.XamlRoot
      };

      ContentDialogResult result = await cd.ShowAsync();
      if (result == ContentDialogResult.Primary)
      {
        bool isSelected = ViewModel.SelectedServer == server;
        int index = ViewModel.Config.Servers.IndexOf(server);
        ViewModel.Config.Servers.Remove(server);
        m_configManager.Save();

        if (isSelected)
          ViewModel.SelectedServer = ViewModel.Config.Servers.Any()
                      ? ViewModel.Config.Servers[index == ViewModel.Config.Servers.Count ? index - 1 : index]
                      : null;
      }
    }

    private void MoveUp_Click(object sender, RoutedEventArgs e)
    {
      Server server = (sender as MenuFlyoutItem).DataContext as Server;

      int index = ViewModel.Config.Servers.IndexOf(server);
      ViewModel.Config.Servers.Move(index, index - 1);
      ViewModel.SelectedServer = server;
      m_configManager.Save();
    }

    private void MoveDown_Click(object sender, RoutedEventArgs e)
    {
      Server server = (sender as MenuFlyoutItem).DataContext as Server;

      int index = ViewModel.Config.Servers.IndexOf(server);
      ViewModel.Config.Servers.Move(index, index + 1);
      ViewModel.SelectedServer = server;
      m_configManager.Save();
    }

    #endregion

    #region Menubar

    #region Launcher

    private async void AddServer_Click(object sender, RoutedEventArgs e)
    {
      AddServerDialog asd = new AddServerDialog(m_configManager.Config)
      {
        XamlRoot = Content.XamlRoot
      };

      ContentDialogResult result = await asd.ShowAsync();
      if (result == ContentDialogResult.Primary)
      {
        ViewModel.Config.Servers.Add(asd.Server);
        ViewModel.SelectedServer = asd.Server;
        m_configManager.Save();
      }
    }

    private async void Settings_Click(object sender, RoutedEventArgs e)
    {
#if DEBUG
      SettingsDialog sd = new SettingsDialog()
      {
        XamlRoot = Content.XamlRoot
      };

      await sd.ShowAsync();
      m_configManager.Save();
#endif
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
      Environment.Exit(0);
    }

    #endregion

    #region Help

    private void Discord_Click(object sender, RoutedEventArgs e)
    {
      Process.Start(new ProcessStartInfo()
      {
        FileName = "https://discord.gg/9KfUdHpUA8",
        UseShellExecute = true
      });
    }

    private void GitHub_Click(object sender, RoutedEventArgs e)
    {
      Process.Start(new ProcessStartInfo()
      {
        FileName = "https://github.com/minisbett/osu-server-launcher",
        UseShellExecute = true
      });
    }

    private void CreateIssue_Click(object sender, RoutedEventArgs e)
    {
      Process.Start(new ProcessStartInfo()
      {
        FileName = "https://github.com/minisbett/osu-server-launcher/issues/new",
        UseShellExecute = true
      });
    }

    #endregion

    #endregion
  }
}
