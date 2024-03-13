using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osuserverlauncher.Infrastructure;
public class DiscordRPCManager
{
    private DiscordRpcClient m_client = null;

    public DiscordRPCManager(DiscordRpcClient client)
    {
        m_client = client;
    }

    public void SetPresence(string details, string state)
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
    }

    public void ClearPresence()
    {
        if (m_client.CurrentPresence != null)
            m_client.ClearPresence();
    }
}
