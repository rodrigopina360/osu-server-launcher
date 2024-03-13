using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using osuserverlauncher.Models;

namespace osuserverlauncher.Utils;

public static class DPAPIUtil
{
    public static string Encrypt(string plain, string entropy)
    {
        byte[] data = Encoding.UTF8.GetBytes(plain);
        string encrypted = Convert.ToBase64String(ProtectedData.Protect(data, Encoding.UTF8.GetBytes(entropy), DataProtectionScope.CurrentUser));
        //Debug.WriteLine($"[Encrypt] {plain} + {entropy} = {encrypted}");
        return encrypted;
    }

    public static string Decrypt(string encrypted, string entropy)
    {
        byte[] data = Convert.FromBase64String(encrypted);
        byte[] decrypted = ProtectedData.Unprotect(data, Encoding.UTF8.GetBytes(entropy), DataProtectionScope.CurrentUser);
        string plain = Encoding.UTF8.GetString(decrypted);
        //Debug.WriteLine($"[Decrypt] {encrypted} + {entropy} = {plain}");
        return plain;
    }
}
