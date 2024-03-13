using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osuserverlauncher.ViewModels;

namespace osuserverlauncher.Utils
{
    public class OsuUtil
    {
    public static void SetServerCredentials(string configFile, ServerViewModel server)
    {
        string[] lines = File.ReadAllLines(configFile);
        for (int i = 0; i < lines.Length; i++)
        if (lines[i].StartsWith("Username ="))
            lines[i] = $"Username = {server.Credentials.Username}";
        else if (lines[i].StartsWith("Password ="))
            lines[i] = $"Password = {server.Credentials.PlainPassword}";
        else if (lines[i].StartsWith("SaveUsername ="))
            lines[i] = $"SaveUsername = 1";
        else if (lines[i].StartsWith("SavePassword ="))
            lines[i] = $"SavePassword = 1";
        else if (lines[i].StartsWith("CredentialEndpoint ="))
            lines[i] = $"CredentialEndpoint = {(server.IsBancho ? "" : server.Domain)}";

        File.WriteAllLines(configFile, lines);
    }

    public static string GetOsuPath()
    {
            if (File.Exists(Path.Combine(Environment.GetEnvironmentVariable("localappdata"), "osu!", "osu!.exe")))
            {
                return Path.Combine(Environment.GetEnvironmentVariable("localappdata"), "osu!");
            }

            try
            {
                string osuexe = Registry.GetValue("HKEY_CLASSES_ROOT\\osu\\DefaultIcon", "", "").ToString().Split('"')[1];
                string osudir = new FileInfo(osuexe).DirectoryName;

                if (!File.Exists(Path.Combine(osudir, "osu!.exe")))
                {
                    return string.Empty;
                }

                return osudir;
            }
            catch 
            {
                return null;
            }
        }
    }
}
