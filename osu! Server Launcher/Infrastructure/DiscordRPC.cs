using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osuserverlauncher.Infrastructure;
public static class DiscordRPC
{
  private static DiscordRpcClient m_client = new DiscordRpcClient("770355757622755379", -1);

  public static bool IsPresenceSet { get; private set; }

  public static void SetPresence(string details, string state)
  {
    if (!m_client.IsInitialized)
      m_client.Initialize();

    RichPresence presence = m_client.CurrentPresence;
    Timestamps timestamp = presence?.Timestamps ?? new Timestamps(DateTime.UtcNow);

    presence = new RichPresence()
    {
      Timestamps = timestamp,
      Details = details,
      State = state,
      Assets = new Assets()
      {
        LargeImageKey = "osu",
        LargeImageText = "Download the osu! server launcher on GitHub!\n\nminisbett/osu-server-launcher"
      }
    };

    m_client.SetPresence(presence);
    IsPresenceSet = true;
  }

  public static void ClearPresence()
  {
    m_client.ClearPresence();
    IsPresenceSet = false;
  }
}
