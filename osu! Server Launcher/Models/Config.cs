using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osuserverlauncher.ViewModels;

namespace osuserverlauncher.Models
{
    public class Config
    {
        [JsonProperty("servers")]
        public List<Server> Servers { get; set; } = new List<Server>();

        [JsonProperty("discord_rpc_enabled")]
        public bool ShowDiscordRPC { get; set; } = false;

        public static Config DefaultConfig => new Config()
        {
            Servers = new List<Server>()
      {
        Server.Bancho
      }
        };
    }
}
