using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osuserverlauncher.Utils
{
  public class StringUtil
  {
    public static string TrimServerDomain(string domain)
    {
      string[] filters = new[]
      {
        "https",
        "http",
        "://",
        "www.",
        "osu."
      };

      foreach (string filter in filters)
        domain = domain.Replace(filter, "");

      return domain.Trim().TrimEnd('/').ToLower();
    }
  }
}
