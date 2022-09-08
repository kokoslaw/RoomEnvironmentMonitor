using System;
using System.IO;
using System.Windows;

namespace RoomEnvironmentMonitor.MVVM.Model
{
    public class GetStartUpSettings
    {
        private const string StartUpSettingsFilePath = "StartUpSettings.txt";
        public string[] startUpSettings;

        public GetStartUpSettings()
        {
            try
            {
                startUpSettings = File.ReadAllLines(StartUpSettingsFilePath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "StartUpSettings file error", MessageBoxButton.OK);
                throw;
            }
        }

    }
}
