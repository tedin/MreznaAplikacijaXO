using System;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace ServerZaXO
{
    class ServerZaXO
    {
        private static TcpListener serverListener;
        private static IPAddress ipAdresaServera;
        private static TcpClient serverClient;
        private static string ipadresa;
        private static string port;
        private static int portServera;
        private static StreamWriter sw;
        private static StreamReader sr;
        private static NetworkStream ns;
        
        /////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////// 
        static String[] ploca = new String[10];
        static String igrajPonovo = "Y";
        static int brojacPoteza = 0;
        ////////////////////////////////////////////////////////////////////////// 
        ///////////////////////////////////////////////////////////////////////// 

        static void Main(string[] args)
        {
            serverListener = new TcpListener(IPAddress.Any, 8435);
            Console.WriteLine("Cekam da se spoji igrac...");
            serverListener.Start();
            serverClient = serverListener.AcceptTcpClient();
            ns = serverClient.GetStream();
            sw = new StreamWriter(ns);
            sw.AutoFlush = true;
            sr = new StreamReader(ns);
            if (serverClient.Connected)
            {
                Console.WriteLine("Klijent spojen");
                sw.WriteLine("Uspjesno ste se spojili!");
                sw.Flush();

            }

            pokreniXO();

            serverClient.Close();
            sw.Close();
            sr.Close();
            Console.WriteLine("Pritisnite ENTER za izlazak...");
            Console.ReadKey();

        }

        static void postaviPlocu()
        {
            for (int i = 0; i < 10; i++)
            {
                ploca[i] = i.ToString();
            }
        }

        static void igrasLiPonovo(String poruka)
        {
            //sw.WriteLine(poruka + " " + "Zelite li igrati ponovo?");
            string pitajprvog = String.Empty;
            string pitajdrugog = String.Empty;
            if (poruka == "Nerijeseno!")
            {
                Console.Clear();
                Console.WriteLine(poruka);
                sw.WriteLine(poruka);
                sw.Flush();
                Console.WriteLine("Zelite li igrati ponovo?");
                sw.WriteLine("Zelite li igrati ponovo?");
                sw.Flush();
                pitajdrugog = sr.ReadLine();
                pitajprvog = Console.ReadLine();
            }
            else if (poruka == "Cestitamo! Pobijedili ste!")
            {
                if (brojX() > brojiO())
                {
                    Console.Clear();
                    Console.WriteLine(poruka);
                    sw.WriteLine("Izgubili ste.");
                    sw.Flush();
                    Console.WriteLine("Zelite li igrati ponovo?");
                    sw.WriteLine("Zelite li igrati ponovo?");
                    sw.Flush();
                    pitajprvog = Console.ReadLine();
                    pitajdrugog = sr.ReadLine();
                }
                else
                {
                    Console.Clear();
                    sw.WriteLine(poruka);
                    sw.Flush();
                    Console.WriteLine("Izgubili ste.");
                    Console.WriteLine("Zelite li igrati ponovo?");
                    sw.WriteLine("Zelite li igrati ponovo?");
                    sw.Flush();
                    pitajdrugog = sr.ReadLine();
                    pitajprvog = Console.ReadLine();
                }
            }
            //sw.WriteLine(poruka);
            //sw.Flush();
            //sw.WriteLine("Zelite li igrati ponovo?");
            //sw.Flush();
            if ((pitajprvog.Equals("Y") || pitajprvog.Equals("y")) && (pitajdrugog.Equals("y") || pitajdrugog.Equals("Y")))
                igrajPonovo = "Y";
            else
                igrajPonovo = "N";
        }

        static void goodBye()
        {
            sw.WriteLine("Navratite opet!");
            sw.Flush();
            Console.WriteLine("Navratite opet!");


        }

        static void unesiZnak(String player)
        {
            Console.Clear();
            Console.WriteLine("Igrac: " + "X");
            sw.WriteLine("Igrac: " + "O");
            sw.Flush();

            string izbori = String.Empty;
            int izbor;
            while (true)
            {
                Console.WriteLine("Brojac poteza: " + brojacPoteza);
                sw.WriteLine("Brojac poteza: " + brojacPoteza);
                sw.Flush();
                iscrtajPlocu();
                if (player == "O")
                {
                    Console.WriteLine("Igrac " + player + " je na potezu...");
                    sw.WriteLine("Izaberite polje: ");
                    sw.Flush();
                    izbori = sr.ReadLine();
                }
                else if (player == "X")
                {
                    sw.WriteLine("Igrac " + player + " je na potezu...");
                    sw.Flush();
                    Console.WriteLine("Izaberite polje: ");
                    izbori = Console.ReadLine();
                    sw.WriteLine(izbori);
                    sw.Flush();
                    Console.Clear();
                }


                izbor = int.Parse(izbori);
                if (izbor < 1 || izbor > 9 || (ploca[izbor].Equals("X") || ploca[izbor].Equals("O")))
                {
                    if (player == "O")
                    {
                        sw.WriteLine("Izbor nije validan, unesite ponovo: ");
                        sw.Flush();
                    }
                    else if (player == "X")
                        Console.WriteLine("Izbor nije validan, unesite ponovo: ");

                }
                else
                    break;
            }
            ploca[izbor] = player;
            brojacPoteza++;
        }

        static void iscrtajPlocu()
        {
            for (int i = 1; i < 8; i += 3)
            {
                if (i == 1)
                {
                    Console.WriteLine("-----------");
                    sw.WriteLine("-----------");
                    sw.Flush();
                }
                Console.WriteLine(" " + ploca[i] + " " + "|" + " " + ploca[i + 1] + " " + "|" + " " + ploca[i + 2]);
                Console.WriteLine("-----------");
                sw.WriteLine(" " + ploca[i] + " " + "|" + " " + ploca[i + 1] + " " + "|" + " " + ploca[i + 2]);
                sw.Flush();
                sw.WriteLine("-----------");
                sw.Flush();
            }
        }

        static Boolean provjeriPobjedu()
        {
            for (int i = 1; i < 8; i += 3)
            {
                if (ploca[i].Equals(ploca[i + 1]) && ploca[i + 1].Equals(ploca[i + 2]))
                {
                    return true;
                }
            }
            if (ploca[1].Equals(ploca[4]) && ploca[4].Equals(ploca[7]))
                return true;
            if (ploca[2].Equals(ploca[5]) && ploca[5].Equals(ploca[8]))
                return true;
            if (ploca[3].Equals(ploca[6]) && ploca[6].Equals(ploca[9]))
                return true;
            if (ploca[3].Equals(ploca[5]) && ploca[5].Equals(ploca[7]))
                return true;
            if (ploca[1].Equals(ploca[5]) && ploca[5].Equals(ploca[9]))
                return true;
            return false;
        }

        static void uvod()
        {
            Console.Title = ("Mrezna aplikacija XO");
            Console.WriteLine("Dobro dosli!");
            Console.WriteLine("Pritisnite ENTER za nastavak...");
            Console.ReadKey();
            sw.WriteLine("Dobro dosli!");
            sw.Flush();
            sw.WriteLine("Pritisnite ENTER za nastavak...");
            sw.Flush();
            string s = sr.ReadLine();
            Console.Clear();
        }

        static void igraj()
        {
            while (igrajPonovo.Equals("Y"))
            {
                postaviPlocu();
                brojacPoteza = 0;
                while (provjeriPobjedu() == false && brojacPoteza < 8)
                {
                    unesiZnak("X");
                    if (provjeriPobjedu() == true)
                        break;
                    unesiZnak("O");
                }
                if (provjeriPobjedu() == true)
                    igrasLiPonovo("Cestitamo! Pobijedili ste!");
                else
                    igrasLiPonovo("Nerijeseno!");
            }
        }

        static void pokreniXO()
        {
            uvod();
            igraj();
            goodBye();
        }

        static int brojX()
        {
            int brojac = 0;
            for (int i = 0; i < ploca.Length; i++)
                if (ploca[i] == "X")
                    brojac++;
            return brojac;
        }

        static int brojiO()
        {
            int brojac = 0;
            for (int i = 0; i < ploca.Length; i++)
                if (ploca[i] == "O")
                    brojac++;
            return brojac;
        }

    }
}
