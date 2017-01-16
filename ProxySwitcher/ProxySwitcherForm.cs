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
        
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private Proxy proxy;

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

            // Get Proxy Settings
            proxy = new Proxy();
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
            var status = proxy.CheckAndRefreshProxy();
            if (status == 1)
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
            proxy.enable();
            CheckAndRefreshProxy();
        }

        private void OnDisableProxy(object sender, EventArgs e)
        {
            proxy.disable();
            CheckAndRefreshProxy();
        }
    }
}
