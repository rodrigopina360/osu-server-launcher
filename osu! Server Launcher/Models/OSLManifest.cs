using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osuserverlauncher.Models;
public class OSLManifest
{
  [JsonProperty("description")]
  public string Description { get; set; } = "";
}
