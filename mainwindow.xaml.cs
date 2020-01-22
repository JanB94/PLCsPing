using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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

        private static List<string> PLCsList()
        {
            List<string> Address = new List<string>();
            Address.Add("192.168.1.1");
            Address.Add("192.168.1.143");
            Address.Add("192.168.1.250");
            Address.Add("192.168.1.122");
            Address.Add("192.168.1.185");
            Address.Add("192.168.1.45");
            Address.Add("192.168.1.66");
            Address.Add("192.168.1.171");
            Address.Add("192.168.1.115");
            Address.Add("192.168.1.178");

            return (Address);
        }
        private async void asyncWorking()
        {
            DataToShow = new List<DataToPlot>();
            bool xFirstLoop = false;
            while (true)
            {

                List<string> slPLCList = PLCsList();
                string sPLCIP;
                double dRespondTime = 0;
                int iThreadSleep = 250;
                int iLoopCount = 1;
                bool xCommunicationStatus = false;
                int TimeOut = 2000;
               
                foreach (string item in slPLCList)
                {
                    sPLCIP = item;
                    await Task.Run(() =>
                    {
                        dRespondTime = PingTimeAverage(sPLCIP, iLoopCount, TimeOut);
                        xCommunicationStatus = CommunicationStatus(dRespondTime);
                        if (xFirstLoop == false)
                        {
                            DataToShow.Add(new DataToPlot(sPLCIP, dRespondTime, xCommunicationStatus));
                        }
                    });
                    System.Threading.Thread.Sleep(iThreadSleep);
                    PLCList.ItemsSource = DataToShow;
                    PLCList.Items.Refresh();
                }
                xFirstLoop = true;

                if (xFirstLoop == true)
                {
                    int j = 0;
                    foreach (string item in slPLCList)
                    {
                        sPLCIP = item;
                        await Task.Run(() =>
                        {
                            dRespondTime = PingTimeAverage(sPLCIP, iLoopCount, TimeOut);
                            xCommunicationStatus = CommunicationStatus(dRespondTime);
                            DataToShow[j] = new DataToPlot(sPLCIP, dRespondTime, xCommunicationStatus);
                        });
                        System.Threading.Thread.Sleep(iThreadSleep);
                        PLCList.ItemsSource = DataToShow;
                        PLCList.Items.Refresh();
                        j++;
                    }
                }
            }
        }

        private void Clock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.Start();
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

        private static double PingTimeAverage(string host, int echoNum, int timeout)
        {
            long totalTime = 0;
            Ping pingSender = new Ping();

            for (int i = 0; i < echoNum; i++)
            {
                PingReply reply = pingSender.Send(host, timeout);
                if (reply.Status == IPStatus.Success)
                {
                    totalTime += reply.RoundtripTime;
                }
                if (reply.Status == IPStatus.TimedOut)
                {
                    totalTime = 9999 * echoNum;
                }
            }
            return totalTime / echoNum;
        }

        private static bool CommunicationStatus(double time)
        {
            bool xStatusOfCommunication;
            if (time < 200)
            {
                xStatusOfCommunication = true;
            }
            else
            {
                xStatusOfCommunication = false;
            }
            return xStatusOfCommunication;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            ClockTime.Content = DateTime.Now.ToLongTimeString();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}