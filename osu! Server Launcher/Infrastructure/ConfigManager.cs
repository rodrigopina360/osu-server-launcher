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
  public class ConfigManager
  {
    public Config Config { get; } = Config.DefaultConfig;

    private string m_path = "";

    public ConfigManager(string path)
    {
      m_path = path;
      Directory.CreateDirectory(new FileInfo(m_path).DirectoryName);
      if (File.Exists(m_path))
        Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(m_path));
      Save();
    }

    public void Save()
    {
      File.WriteAllText(m_path, JsonConvert.SerializeObject(Config, Formatting.Indented));
    }
  }
}
