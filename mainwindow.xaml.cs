using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Newtonsoft.Json;

namespace PLCsPing
{
    public partial class MainWindow : Window
    {
        private List<DataToPlot> DataToShow = null;
        public MainWindow()
        {
            InitializeComponent();
            Clock();
            asyncWorking();
        }
        private async void asyncWorking()
        {
            DataToShow = new List<DataToPlot>();
            string sPLCIP;
            int TimeOut = 0;
            int iThreadSleep = 0;
            int iLoopCount = 0;
            double dRespondTime = 0;
            bool xCommunicationStatus = false;
            bool xFirstLoop = false;

            List<string> slPLCList = PLCsList(out TimeOut, out iThreadSleep, out iLoopCount);
            if(iThreadSleep < 50)
            {
                iThreadSleep = 50;
            }
            do
            {
                try
                {
                    int j = 0;
                    foreach (string item in slPLCList)
                    {
                        sPLCIP = item;
                        
                        if (xFirstLoop == true)
                        {
                            await Task.Run(() =>
                            {
                                dRespondTime = PingTimeAverage(sPLCIP, iLoopCount, TimeOut);
                                xCommunicationStatus = CommunicationStatus(dRespondTime);
                                DataToShow[j] = new DataToPlot(sPLCIP, dRespondTime, xCommunicationStatus);
                                j++;
                            });
                            PLCList.ItemsSource = DataToShow;
                            PLCList.Items.Refresh();
                            System.Threading.Thread.Sleep(iThreadSleep);
                        }
                        if (xFirstLoop == false)
                        {
                            await Task.Run(() =>
                            {
                                dRespondTime = PingTimeAverage(sPLCIP, iLoopCount, TimeOut);
                                xCommunicationStatus = CommunicationStatus(dRespondTime);
                                DataToShow.Add(new DataToPlot(sPLCIP, dRespondTime, xCommunicationStatus));
                            });
                        }
                    }
                    if (xFirstLoop == false)
                    {
                        PLCList.ItemsSource = DataToShow;
                        xFirstLoop = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A handled exception just occurred: \n" + ex.Message, "Exception number 0x0001", MessageBoxButton.OK, MessageBoxImage.Error);
                    System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
                    process.Kill();
                }
            } while (xFirstLoop == true);
        }
        private static List<string> PLCsList(out int iTimeOut, out int iThreadSleep, out int iLoopCount)
        {
            List<string> Address = new List<string>();
            iTimeOut = 1;
            iThreadSleep = 1;
            iLoopCount = 1;

            try
            {
                string SciezkaDoPlikuZapisu = @"PLCList.json";
                string Data = System.IO.File.ReadAllText(SciezkaDoPlikuZapisu);
                var PLCAdresIP = JsonConvert.DeserializeObject<List<PLCAdresIP>>(Data);
                Address = PLCAdresIP[0].AdresIP;
                iThreadSleep = Int32.Parse(PLCAdresIP[0].ThreadSleep);
                iLoopCount = Int32.Parse(PLCAdresIP[0].LoopCount);
                iTimeOut = Int32.Parse(PLCAdresIP[0].TimeOut);
            }
            catch (Exception ex)
            {
                MessageBox.Show("A handled exception just occurred: \n" + ex.Message, "Exception number 0x0002", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
                process.Kill();
            }
            return (Address);
        }
        private void Clock()
        {
            try
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(100);
                timer.Tick += timer_Tick;
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("A handled exception just occurred: \n" + ex.Message, "Exception number 0x0003", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
                process.Kill();
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            ClockTime.Content = DateTime.Now.ToLongTimeString();
        }
        private static double PingTimeAverage(string host, int echoNum, int iTimeOut)
        {
            long totalTime = 0;
            Ping pingSender = new Ping();
            try
            {
                for (int i = 0; i < echoNum; i++)
                {
                    PingReply reply = pingSender.Send(host, iTimeOut);
                    if (reply.Status == IPStatus.Success)
                    {
                        totalTime += reply.RoundtripTime;
                    }
                    if (reply.Status == IPStatus.TimedOut)
                    {
                        totalTime = 9999 * echoNum;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("A handled exception just occurred: \n" + ex.Message, "Exception number 0x0004", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
                process.Kill();
            }
            return totalTime / echoNum;
        }
        private static bool CommunicationStatus(double time)
        {
            bool xStatusOfCommunication = false;
            try
            {
                if (time < 200)
                {
                    xStatusOfCommunication = true;
                }
                else
                {
                    xStatusOfCommunication = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("A handled exception just occurred: \n" + ex.Message, "Exception number 0x0005", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
                process.Kill();
            }
            return xStatusOfCommunication;
        }
        private class DataToPlot
        {
            public string AddresIP { get; set; }
            public double ResponseTime { get; set; }
            public bool CommunicationStatus { get; set; }

            public DataToPlot(string nAddresIP, double nResponseTime, bool nCommunicationStatus)
            {
                AddresIP = nAddresIP;
                ResponseTime = nResponseTime;
                CommunicationStatus = nCommunicationStatus;
            }
        }
        private class PLCAdresIP
        {
            public string TimeOut;
            public string ThreadSleep;
            public string LoopCount;
            public List<string> AdresIP;
        }
    }
}