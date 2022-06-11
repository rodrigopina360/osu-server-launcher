using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using osuserverlauncher.Models;
using osuserverlauncher.Infrastructure;

namespace osuserverlauncher.ViewModels
{
  public class ServerViewModel : PropertyChangedBase
  {

    #region Offline server infos

    private string m_name = "";
    public string Name
    {
      get => m_name;
      set => SetField(ref m_name, value);
    }

    private string m_domain = "";
    public string Domain
    {
      get => m_domain;
      set => SetField(ref m_domain, value);
    }

    private DateTime m_lastPlayed = DateTime.MinValue;
    public DateTime LastPlayed
    {
      get => m_lastPlayed;
      set => SetField(ref m_lastPlayed, value);
    }

    private Credentials m_credentials = null;
    public Credentials Credentials
    {
      get => m_credentials;
      set => SetField(ref m_credentials, value);
    }

    #endregion

    #region Icon URLs

    private string m_osuIcon = "https://i.ppy.sh/013ed2c11b34720790e74035d9f49078d5e9aa64/68747470733a2f2f6f73752e7070792e73682f77696b692f696d616765732f4272616e645f6964656e746974795f67756964656c696e65732f696d672f75736167652d66756c6c2d636f6c6f75722e706e67";

    public string Icon32Url
    {
      get
      {
        if (IsBancho)
          return m_osuIcon;

        return $"https://{Domain}/static/osl/favicon32.png";
      }
    }

    public string Icon64Url
    {
      get
      {
        if (IsBancho)
          return m_osuIcon;

        return $"https://{Domain}/static/osl/favicon64.png";
      }
    }

    #endregion

    #region Bancho

    public bool IsBancho => m_domain == Server.Bancho.Domain;

    #endregion

    #region Manifest

    public async Task<OSLManifest> GetManifest()
    {
      if (IsBancho)
        return m_banchoManifest;

      if (!m_manifestFetched)
        try
        {
          m_manifestFetched = true;
          string json = await m_client.GetStringAsync($"https://{Domain}/static/osl/manifest.json");
          m_manifest = JsonConvert.DeserializeObject<OSLManifest>(json);
        }
        catch { }

      return m_manifest;
    }

    private bool m_manifestFetched = false;
    private HttpClient m_client = new HttpClient();
    private OSLManifest m_manifest = new OSLManifest();
    private OSLManifest m_banchoManifest = new OSLManifest()
    {
      Description = "This is the official osu! server, hosted by peppy."
    };

    #endregion

    public ServerViewModel(Server server)
    {
      Name = server.Name;
      Domain = server.Domain;
      LastPlayed = server.LastPlayed;
      Credentials = server.Credentials;
    }

    public ServerViewModel() { }

    public ServerViewModel Clone()
    {
      return new ServerViewModel()
      {
        Name = m_name,
        Domain = m_domain,
        LastPlayed = m_lastPlayed,
        Credentials = m_credentials,
        m_manifest = m_manifest
      };
    }

    public Server ToModel()
    {
      return new Server(Name, Domain, LastPlayed, Credentials);
    }
  }
}
