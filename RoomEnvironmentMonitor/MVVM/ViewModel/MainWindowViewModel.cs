using System.IO.Ports;
using RoomEnvironmentMonitor.MVVM.Model;
using System;
using System.Windows;
//using System.Diagnostics;
using System.Timers;

namespace RoomEnvironmentMonitor.MVVM.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        private string temperature = "???";
        public string Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }

        private string light = "???";
        public string Light
        {
            get { return light; }
            set
            {
                light = value;
                OnPropertyChanged(nameof(Light));
            }
        }

        private string humidity = "???";
        public string Humidity
        {
            get { return humidity; }
            set
            {
                humidity = value;
                OnPropertyChanged(nameof(Humidity));
            }
        }

        private readonly GetStartUpSettings SUS; // <--- impostor???
        private readonly SerialPort serialPort;
        private readonly Timer CheckTimer;
        public MainWindowViewModel()
        {
            SUS = new();
            serialPort = new(SUS.startUpSettings[3], int.Parse( SUS.startUpSettings[4] ));
            CheckTimer = new(int.Parse( SUS.startUpSettings[5] ));

            CheckTimer.AutoReset = true;
            CheckTimer.Elapsed += CheckTimer_Elapsed;
            serialPort.DataReceived += SerialPort_DataReceived;

            TryToConnect(serialPort);
            CheckTimer.Start();

        }
        private void CheckTimer_Elapsed(object? sender, ElapsedEventArgs e) 
        {
            try
            {
                serialPort.WriteLine("SendData!");
            }
            catch (Exception)
            {
                TryToConnect(serialPort, CheckTimer);
            }
        } 

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string receivedData = serialPort.ReadLine();
            string tempBuffer = default!;
            string lightBuffer = default!;
            string humiBuffer = default!;

            tempBuffer += receivedData[0];
            tempBuffer += receivedData[1];
            humiBuffer += receivedData[2];
            humiBuffer += receivedData[3];

            // The light level value can vary in length
            for (int i = 4; i < receivedData.Length - 1; i++)
                lightBuffer += receivedData[i];

            Temperature = tempBuffer;
            Light = lightBuffer;
            Humidity = humiBuffer;
        }

        private void TryToConnect(SerialPort serialPort , Timer? timer = null)
        {
        TryAgain:
            try
            {
                serialPort.Open();
            }
            catch (Exception e)
            {
                timer?.Stop();
                if (MessageBox.Show(e.Message.ToString() + "\ntry again?", "COM error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    goto TryAgain;
                else throw;
            }
            timer?.Start();
        }

    }
}
