using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using RoomEnvironmentMonitor.MVVM.Model;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;

namespace RoomEnvironmentMonitor.Behaviors
{
    public class MainWindowBehaviors : Behavior<Window>
    {
        private Point WindowStartPosition;
        private bool isWindowMoving = false;
        private const string StartUpSettingsFilePath = "StartUpSettings.txt";
        GetStartUpSettings startUpSettings;

        public MainWindowBehaviors() => startUpSettings = new();

        protected override void OnAttached()
        {
            if (this.AssociatedObject is Window window)
            {
                window.MouseDown += Window_MouseDown;
                window.MouseUp += Window_MouseUp;
                window.MouseMove += Window_MouseMove;
                window.Closing += Window_Closing;
                window.Loaded += Window_Loaded;
            }
        }
        #region ReadWriteSetupFile
        private void Window_Closing(object? sender, CancelEventArgs e)
        {
            Window? window = sender as Window;
            startUpSettings.startUpSettings[0] = window.Opacity.ToString().Replace(",", ".");
            startUpSettings.startUpSettings[1] = window.Left.ToString();
            startUpSettings.startUpSettings[2] = window.Top.ToString();

            File.WriteAllLines(StartUpSettingsFilePath, startUpSettings.startUpSettings);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Window? window = sender as Window;

            window.Opacity = double.Parse( startUpSettings.startUpSettings[0], NumberStyles.Any, CultureInfo.InvariantCulture);
            window.Left = double.Parse( startUpSettings.startUpSettings[1] );
            window.Top = double.Parse( startUpSettings.startUpSettings[2] );
        }
        #endregion

        #region DragAndDropLogic
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!isWindowMoving & e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                Window window = (Window)sender;
                isWindowMoving = true;
                WindowStartPosition = e.GetPosition(window);
            }
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isWindowMoving)
            {
                Window window = (Window)sender;
                Point CursorLocation = e.GetPosition(window);
                Vector move = CursorLocation - WindowStartPosition;
                window.Top += move.Y;
                window.Left += move.X;
            }
        }

        private void Window_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isWindowMoving & e.LeftButton == System.Windows.Input.MouseButtonState.Released)
                isWindowMoving = false;
        }
        #endregion
    }
}
