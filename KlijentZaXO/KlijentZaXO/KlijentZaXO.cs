using System;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace KlijentZaXO
{

    class KlijentZaXO
    {
        private static TcpClient klijent;
        private static string ipAdresa;
        private static IPAddress adresaServera;
        private static string portDefaultni = "8435";
        private static string port;
        private static int portServera;
        private static StreamReader sr;
        private static StreamWriter sw;
        private static NetworkStream ns;


        static void Main(string[] args)
        {
            Console.Title = ("Klijent za XO");
            Console.WriteLine("Unesite IP adresu servera (ili pritisnite ENTER ukoliko je server na loopback adresi): ");
            ipAdresa = Console.ReadLine();
            if (ipAdresa == string.Empty)
                ipAdresa = "127.0.0.1";
            adresaServera = IPAddress.Parse(ipAdresa);
            Console.WriteLine("Unesite port servera (ili pritisnite ENTER ukoliko je server na loopback adresi): ");
            port = Console.ReadLine();
            if (port == string.Empty)
                portServera = int.Parse(portDefaultni);
            else
                portServera = int.Parse(port);
            try
            {
                klijent = new TcpClient();
                klijent.Connect(adresaServera, portServera);
            }
            catch (Exception e)
            {

                Console.WriteLine("Nije se moguce spojiti na tu adresu, greska" + e.Message);
                Console.ReadKey();
            }
            if (klijent.Connected)
            {
                try
                {
                    ns = klijent.GetStream();
                    sw = new StreamWriter(ns);
                    sw.AutoFlush = true;
                    sr = new StreamReader(ns);
                    Console.WriteLine(sr.ReadLine());
                }
                catch (Exception e)
                {

                    Console.WriteLine("Doslo je do greske: " + e.Message);
                    Console.ReadKey();
                }

            }
            string primljenaporuka = sr.ReadLine();
            string poruka;

            while (true)
            {
                Console.WriteLine(primljenaporuka);

                if (primljenaporuka == "Zelite li igrati ponovo?")
                {
                    poruka = Console.ReadLine();
                    sw.WriteLine(poruka);
                    sw.Flush();
                    Console.Clear();
                }
                else if (primljenaporuka == "Pritisnite ENTER za nastavak...")
                {

                    Console.ReadKey();
                    sw.WriteLine();
                    sw.Flush();
                    Console.Clear();
                }
                else if (primljenaporuka == "Izaberite polje: ")
                {

                    string izbor = Console.ReadLine();
                    sw.WriteLine(izbor);
                    sw.Flush();
                    Console.Clear();
                }
                else if (primljenaporuka == "Zelite li igrati ponovo?")
                {
                    string izbor = Console.ReadLine();
                    Console.Clear();
                    sw.WriteLine(izbor);
                    sw.Flush();
                }
                else if (primljenaporuka == "Igrac X je na potezu...")
                {
                    sr.ReadLine();
                    Console.Clear();
                }
                else if (primljenaporuka == "Navratite opet!")
                {
                    break;
                }
                primljenaporuka = sr.ReadLine();
            }

            klijent.Close();
            sr.Close();
            sw.Close();
            Console.WriteLine("Pritisnite ENTER za izlazak...");
            Console.ReadKey();
        }
    }
}
