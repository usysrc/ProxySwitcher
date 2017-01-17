using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwitcher
{
    class Startup
    {
        string AppName = "ProxySwitcher";
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
              ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public bool isEnabled()
        {
           
            return registryKey.GetValue(AppName) != null;
        }

        public void Enable()
        {
            registryKey.SetValue(AppName, AppDomain.CurrentDomain.BaseDirectory.ToString());
        }

        public void Disable()
        {
            registryKey.DeleteValue(AppName, false);
        }
    }
}
