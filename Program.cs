using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics;

namespace Examples.System.Net.NetworkInformation.PingTest
{
    public class PingExample
    {
        // args[0] can be an IPaddress or host name.
        public static void Main(string[] args)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;

            PingReply reply = pingSender.Send("192.168.1.1", timeout, buffer, options);

            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", reply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
            }

            Odległość p = new Odległość();
            p.WartośćMetr = 12;
            Console.WriteLine("Buffer size: {0}", p.WartośćMila);

            int? liczba = null;
 
            Console.WriteLine("[1] {0}", liczba ?? 3);
            liczba += 12;
            Console.WriteLine("[2] {0}", liczba ?? 1);
            liczba = 1234;
            Console.WriteLine("[3] {0}", liczba);
            liczba += 12;
            Console.WriteLine("[4] {0}", liczba);

            int liczba32 = 3;
            long liczba64 = 3L;

            Console.WriteLine("{0} {1}", liczba32, liczba32.GetType());
            Console.WriteLine("{0} {1}", liczba64, liczba64.GetType());

            static int iloczyn(int a, int b, int c = 1)
            {
                return a * b * c;
            }

            float? wynik; 

            wynik = iloczyn(b: 7, a: 8);
            Console.WriteLine("{0}", wynik);

            var zmienna1 = new { A = 1, B = 2 };
            Console.WriteLine(zmienna1);


            int[] dane = { 1, 5, 3, 2, 4 };
            int max = dane[0], min = dane[0];
            double średnia = dane[0];
            for (int i = 1; i < dane.Length; i++)
            {
                if (dane[i] > max)
                {
                    max = dane[i];
                }
                else if (dane[i] < min)
                {
                    min = dane[i];
                }
                średnia += dane[i];
            }
            średnia /= dane.Length;
            var podsumowanie = new { max, min, średnia };
            Console.WriteLine(podsumowanie);

/*
            string tekst = "";
            for (int i = 0; i < 10000; i++)
            {
                tekst += "Uczę się jak szybko tworzyć teksty w języku C#\n";
            }
*/
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 10000; i++)
            {
                builder.Append("Uczę się jak szybko tworzyć teksty w języku C#\n");
            }


            StringBuilder tekst = new StringBuilder();
            tekst.Append("Pierwsza linijka\n");
            tekst.AppendLine("Druga linijka");
            tekst.Append('*', 10);
            Console.WriteLine(tekst.ToString());



            tekst.Remove(0, tekst.Length);
            tekst.Append("Nowy do wyświetlenia");
            tekst.Insert(5, "tekst ");
            Console.WriteLine(tekst.ToString());


            static int[,] GenerujTabliczke(int n)
            {
                int[,] tab = new int[n, n];
                for (int x = 0; x < n; x++)
                {
                    for (int y = 0; y < n; y++)
                    {
                        tab[x, y] = x * y;
                    }
                }
                return tab;
            }


            Stopwatch stopWatch = new Stopwatch();
            TimeSpan min2 = new TimeSpan(long.MaxValue);
            TimeSpan max2 = new TimeSpan(0);
            Console.WriteLine("Min.\t\t\tTeraz\t\t\tMax.");
            Console.Write("Rozpocznij wciskanie guzików...\n");
            Console.ReadKey();

            ConsoleKeyInfo key;
            while (true)
            {
                stopWatch.Reset();
                stopWatch.Start();
                key = Console.ReadKey();
                stopWatch.Stop();
                if (min2 > stopWatch.Elapsed)
                {
                    min2 = stopWatch.Elapsed;
                }
                else if (stopWatch.Elapsed > max2)
                {
                    max2 = stopWatch.Elapsed;
                }
                if(key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                Console.Write("\r{0}\t{1}\t{2} ", min, stopWatch.Elapsed, max);
            }


        }
        class Odległość
        {
            private const double mn_mila = 8554.3;
            private double odl = 0;
            public double WartośćMetr
            {
                get { return odl; }
                set { odl = value; }
            }
            public double WartośćMila
            {
                get { return odl / mn_mila; }
                set { odl = mn_mila * value; }
            }
        }
    }
}

