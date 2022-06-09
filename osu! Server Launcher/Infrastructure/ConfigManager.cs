using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osuserverlauncher.Models;

namespace osuserverlauncher.Infrastructure
{
  public static class ConfigManager
  {
    public static Config Config { get; private set; } = Config.DefaultConfig;

    private static string m_path = "";

    public static void Initialize(string path)
    {
      m_path = path;
    }

    public static void Load()
    {
      Directory.CreateDirectory(new FileInfo(m_path).DirectoryName);
      if (!File.Exists(m_path))
        File.WriteAllText(m_path, JsonConvert.SerializeObject(Config));
      else
        Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(m_path));
    }

    public static void Save()
    {
      File.WriteAllText(m_path, JsonConvert.SerializeObject(Config, Formatting.Indented));
    }
  }
}
