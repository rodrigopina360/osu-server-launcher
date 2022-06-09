using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osuserverlauncher.Infrastructure;
using osuserverlauncher.Utils;
using osuserverlauncher.ViewModels;

namespace osuserverlauncher.Models;
public class Credentials
{
  [JsonProperty("username")]
  public string Username { get; set; }

  [JsonProperty("password")]
  public string EncryptedPassword
  {
    get => DPAPIUtil.Encrypt(m_password, "cu24180ncjeiu0ci1nwui");
    set => m_password = DPAPIUtil.Decrypt(value, "cu24180ncjeiu0ci1nwui");
  }

  private string m_password = "";
  [JsonIgnore]
  public string PlainPassword
  {
    get => m_password;
    set => m_password = value;
  }

  [Obsolete]
  [JsonConstructor]
  public Credentials() { }

  public Credentials(string username = "", string plainPassword = "")
  {
    Username = username;
    PlainPassword = plainPassword;
  }
}
