using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using KursWalutLibrary;
namespace KursyWalut
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() == 3)
            {
                //Console.WriteLine("Podaj kod waluty (USD, EUR, CHF, GBP)");
                //string _currencyCode = Console.ReadLine().ToUpper();
                string _currencyCode = args[0].ToUpper();

                //Console.WriteLine("Podaj date poczatkowa [RRRR-MM-DD]");
                //string _startDate = Console.ReadLine();
                string _startDate = args[1];

                //Console.WriteLine("Podaj date koncowa [RRRR-MM-DD]");
                //string _endDate = Console.ReadLine();
                string _endDate = args[2];

                DateTime startDate = DateTime.Parse(_startDate);
                DateTime endDate = DateTime.Parse(_endDate);

                string moneycode = _currencyCode;

                KursWalut nowykurs = new KursWalut();
                nowykurs.SetDatesList(startDate, endDate);
                Console.WriteLine("INITIALIZE...");
                nowykurs.Initialize(moneycode);


                float srednia = KursWalut.ObliczSrednia(nowykurs._money);

                Console.WriteLine("Sredni kurs: " + srednia.ToString("n4"));

                float odchylenie = KursWalut.LiczOdchylenieStandardowe(nowykurs._money, srednia);
                Console.WriteLine("Odchylenie standardowe: " + odchylenie.ToString("n4"));

                float kursminimalny = KursWalut.KursMinimalny(nowykurs._money);
                Console.WriteLine("Kurs Minimalny: " + kursminimalny.ToString("n4"));

                float kursmaksymalny = KursWalut.KursMaksymalny(nowykurs._money);
                Console.WriteLine("Kurs Maksymalny: " + kursmaksymalny.ToString("n4"));

                var roznica = KursWalut.RoznicaKursu(nowykurs._money);
                Console.WriteLine("Najwieksza roznica: " + roznica.Item2 + " Dnia: " + roznica.Item1.ToString("yyyy/MM/dd"));


                Console.WriteLine("FINISHED");
                Console.ReadKey();
            }
        }
        
    }
}
