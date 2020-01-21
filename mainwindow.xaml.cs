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
        private List<DataToPlot> DataToSend = null;
        public MainWindow()
        {

            InitializeComponent();
            Clock();
            asyncWorking();

            InitBinding();

        }
        private void InitBinding()
        {
/*
            DataToSend = new List<Person>();
            DataToSend.Add(new Person(1, "Jan", "Kowalski", 25));
            DataToSend.Add(new Person(2, "Adam", "Nowak", 24));
            DataToSend.Add(new Person(3, "Agnieszka", "Kowalczyk", 23));

            lstPersons.ItemsSource = m_oPersonList;*/
        }


        public static List<string> PLCsList()
        {
            List<string> Address = new List<string>();
            Address.Add("192.168.1.1");
            Address.Add("192.168.100.81");
            Address.Add("192.168.100.82");
            Address.Add("192.168.100.83");
            return (Address);
        }
        private async void asyncWorking()
        {
            DataToSend = new List<DataToPlot>();
            bool xFirstLoop = false;
            while (true)
            {

                List<string> slPLCList = PLCsList();
                string sPLCIP;
                double dRespondTime = 0;
                int iThreadSleep = 50;
                int iLoopCount = 1;
                bool xCommunicationStatus = false;
               
                foreach (string item in slPLCList)
                {
                    sPLCIP = item;
                    for (int i = 0; i < iLoopCount; i++)
                    {
                        await Task.Run(() =>
                        {
                            dRespondTime = PingTimeAverage(sPLCIP, 2);
                            xCommunicationStatus = CommunicationStatus(dRespondTime);
                            System.Threading.Thread.Sleep(iThreadSleep);
                            if (xFirstLoop == false)
                            {
                                DataToSend.Add(new DataToPlot(sPLCIP, dRespondTime, xCommunicationStatus));
                            }
                        });
                    }

                }
                if(xFirstLoop == false)
                {
                    lstPersons.ItemsSource = DataToSend;
                    xFirstLoop = true;
                }
                // <!-- TERAZ TU ZROBIC ZEBY AKTUALIZOWAC LISTE --!>
            }
        }

        private void Clock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        public class Person
        {
            public int PersonId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }

            public Person(int nPersonId, string sFirstName, string sLastName,
                int nAge)
            {
                PersonId = nPersonId;
                FirstName = sFirstName;
                LastName = sLastName;
                Age = nAge;
            }
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

        void timer_Tick(object sender, EventArgs e)
        {
            ClockTime.Content = DateTime.Now.ToLongTimeString();
        }

        private static double PingTimeAverage(string host, int echoNum)
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

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}