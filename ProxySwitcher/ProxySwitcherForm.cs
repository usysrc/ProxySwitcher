using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ProxySwitcher
{
    public partial class ProxySwitcherForm : Form
    {
        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        static bool settingsReturn, refreshReturn;

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);

        public ProxySwitcherForm()
        {
            InitializeComponent();

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Properties", OnProperties);
            trayMenu.MenuItems.Add("EnableProxy", OnEnableProxy);
            trayMenu.MenuItems.Add("DisableProxy", OnDisableProxy);
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Create a tray icon
            trayIcon = new NotifyIcon();
            trayIcon.Text = "ProxySwitcher";

            // Add MouseOver Handler
            trayIcon.MouseMove += TrayIcon_MouseMove;
            trayIcon.Icon = ProxySwitcher.Properties.Resources.bild;
            
            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;

            CheckAndRefreshProxy();
        }

        private void OnProperties(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("inetcpl.cpl", ",4");
        }

        private void TrayIcon_MouseMove(object sender, MouseEventArgs e)
        {
            CheckAndRefreshProxy();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        private void CheckAndRefreshProxy()
        {
            // Refresh System
            settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);

            // Check Proxy Reg Key
            int proxyStatus = (int)registry.GetValue("ProxyEnable");
            if (proxyStatus == 1)
            {
                trayIcon.Text = "ProxySwitcher>Proxy On";
                trayIcon.Icon = ProxySwitcher.Properties.Resources.connect;
            }
            else
            {
                trayIcon.Icon = ProxySwitcher.Properties.Resources.bild;
                trayIcon.Text = "ProxySwitcher>Proxy Off";
            }
                
        }

        private void OnEnableProxy(object sender, EventArgs e)
        {
            registry.SetValue("ProxyEnable", 1);
            CheckAndRefreshProxy();
        }

        private void OnDisableProxy(object sender, EventArgs e)
        {
            registry.SetValue("ProxyEnable", 0);
            CheckAndRefreshProxy();
        }



    }
}
