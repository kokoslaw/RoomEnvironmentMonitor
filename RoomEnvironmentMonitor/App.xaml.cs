using System;
using System.Drawing;
using System.Windows;
using Forms = System.Windows.Forms;

namespace RoomEnvironmentMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private readonly string IconPath = @"Icon.ico";
        readonly Forms.NotifyIcon notifyIcon;

        public App()
        {
            notifyIcon = new();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            notifyIcon.Icon = new Icon(IconPath);
            notifyIcon.Visible = true;
            notifyIcon.ContextMenuStrip = new();
            notifyIcon.ContextMenuStrip.Items.Add("Exit", Image.FromFile(IconPath), OnStartup);
            base.OnStartup(e);
        }

        private void OnStartup(object? sender, EventArgs e) => Shutdown();

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose();
            base.OnExit(e);
        }

    }
}
