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


namespace PLCsPing
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {

            InitializeComponent();

            List<string> slPLCList = PLCsList();
            string sPLCIP;
            double dRespondTime = 0;
            int iThreadSleep = 50;
            int iLoopCount = 1;
            bool xCommunicationStatus = false;

            foreach (string item in slPLCList)
            {
                DataToPlot DataToPlot = new DataToPlot();
                sPLCIP = item;
                DataToPlot.AddresIP = sPLCIP;
                for (int i = 0; i<iLoopCount; i++)
                {
                    dRespondTime = PingTimeAverage(sPLCIP, 2);
                    DataToPlot.ResponseTime = dRespondTime;
                    xCommunicationStatus = CommunicationStatus(dRespondTime);
                    DataToPlot.CommunicationStatus = xCommunicationStatus;
                    System.Threading.Thread.Sleep(iThreadSleep);
                }
                PLCsDataGrid.Items.Add(DataToPlot);
            }
            
        }

        public static List<string> PLCsList()
        {
            List<string> Address = new List<string>();
            Address.Add("192.168.1.1");
            Address.Add("192.168.1.2");
            Address.Add("192.168.1.3");
            Address.Add("192.168.1.4");
            Address.Add("192.168.1.5");
            Address.Add("192.168.1.6");
            Address.Add("192.168.1.7");
            Address.Add("192.168.1.8");
            Address.Add("192.168.1.9");
            Address.Add("192.168.1.10");
            Address.Add("192.168.1.11");
            Address.Add("192.168.1.12");
            return (Address);
        }

        public class DataToPlot
        {
            public string AddresIP { get; set; }
            public double ResponseTime { get; set; }
            public bool CommunicationStatus { get; set; }
        }


        public static double PingTimeAverage(string host, int echoNum)
        {
            long totalTime = 0;
            int timeout = 120;
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

        public static bool CommunicationStatus(double time)
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


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}