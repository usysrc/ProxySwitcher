using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwitcher
{
    internal static class NativeMethods
    {
        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
    }

    class Proxy
    {
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        static bool settingsReturn, refreshReturn;

        enum ProxyStatus
        {
            Off = 0,
            On = 1
        }
        string KeyValue = "ProxyEnable";
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);

        public void Refresh()
        {
            // Refresh System
            settingsReturn = NativeMethods.InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            refreshReturn = NativeMethods.InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        public bool isEnabled()
        {
            // Check Proxy Reg Key
            int proxyStatus = (int) registryKey.GetValue(KeyValue);
            return proxyStatus == (int) ProxyStatus.On;
        }

        public void Enable()
        {
            registryKey.SetValue(KeyValue, 1);
        }

        public void Disable()
        {
            registryKey.SetValue(KeyValue, 0);
        }
    }
}
