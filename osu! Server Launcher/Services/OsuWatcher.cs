using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using osuserverlauncher.ViewModels;
using osuserverlauncher.Infrastructure;

namespace osuserverlauncher.Services;
public static class OsuWatcher
{
  private static DispatcherQueueTimer m_timer = null;

  public static void Start(MainViewModel viewModel)
  {
    m_timer = DispatcherQueueController.CreateOnDedicatedThread().DispatcherQueue.CreateTimer();
    m_timer.Interval = TimeSpan.FromSeconds(1);
    m_timer.Tick += (sender, e) =>
    {
      Process osu = Process.GetProcessesByName("osu!").FirstOrDefault();
      viewModel.IsOsuRunning = osu != null;

      if (viewModel.IsOsuRunning && viewModel.ConnectedServer != null /* !null -> connected through server launcher */)
      {
        string title = osu.MainWindowTitle;
        string details = $"{viewModel.ConnectedServer.Credentials?.Username ?? "?"} - {viewModel.ConnectedServer.Domain}";

        if (title == "osu!")
          Infrastructure.DiscordRPC.SetPresence(details, "Idle");
        else if (title.StartsWith("osu!  -"))
            Infrastructure.DiscordRPC.SetPresence(details, $"{(title.EndsWith(".osu") ? "Editing" : "Playing")} {title.Substring(8)}");
      }
      else
      {
        viewModel.ConnectedServer = null;

        if (Infrastructure.DiscordRPC.IsPresenceSet)
          Infrastructure.DiscordRPC.ClearPresence();
      }
    };

    m_timer.Start();
  }
}
