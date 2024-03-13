using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osuserverlauncher.Models
{
    public class Server
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("domain")]
        public string Domain { get; private set; }

        [JsonProperty("last_played")]
        public DateTime LastPlayed { get; private set; } = DateTime.MinValue;

        [JsonProperty("credentials")]
        public Credentials Credentials { get; private set; }

        public static Server Bancho => new Server()
        {
            Name = "Bancho",
            Domain = "osu.ppy.sh"
        };

        [JsonConstructor]
        private Server() { }

        public Server(string name, string domain, DateTime lastPlayed, Credentials credentials)
        {
            Name = name;
            Domain = domain;
            LastPlayed = lastPlayed;
            Credentials = credentials;
        }
    }
}
