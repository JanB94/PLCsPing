using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PLCsPing
{
    class Program
    {
        static void Main(string[] args)
        {

            string SciezkaDoPlikuZapisu = @"..\..\PLCList.json";
            string Data = System.IO.File.ReadAllText(SciezkaDoPlikuZapisu);
            PlcList plcList = JsonConvert.DeserializeObject<PlcList>(Data);

            string PLCIP;
            PLCIP = plcList.Adres1;
            double dRespondTime = 0;

            Console.WriteLine("Pingowany sterownik: {0}", PLCIP);
            for (int i = 0; i < 4; i++)
            {
                dRespondTime = PingTimeAverage(PLCIP, 2);
                Console.WriteLine("Odpowiedź z adresu: {0} wynosi {1}ms - {2}", PLCIP, dRespondTime, CommunicationStatus(dRespondTime));
                System.Threading.Thread.Sleep(1000);
            }
            Console.WriteLine("Koniec pingowania: {0}", PLCIP);
            Console.ReadKey();
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
        public partial class PlcList
        {
            [JsonProperty("Adres1")]
            public string Adres1 { get; set; }

            [JsonProperty("Adres2")]
            public string Adres2 { get; set; }
        }
    }
}
