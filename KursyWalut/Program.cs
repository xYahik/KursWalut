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
            Console.WriteLine("Podaj kod waluty (USD, EUR, CHF, GBP)");
            string _currencyCode = Console.ReadLine();

            Console.WriteLine("Podaj date poczatkowa [RRRR-MM-DD]");
            string _startDate = Console.ReadLine();

            Console.WriteLine("Podaj date koncowa [RRRR-MM-DD]");
            string _endDate = Console.ReadLine();

            DateTime startDate = DateTime.Parse(_startDate);
            DateTime endDate = DateTime.Parse(_endDate);
            string moneycode = _currencyCode;

            KursWalut nowykurs = new KursWalut();
            nowykurs.SetDatesList(startDate, endDate);
            Console.WriteLine("INITIALIZE...");
            nowykurs.Initialize(moneycode);

            
            float srednia = KursWalut.ObliczSrednia(nowykurs._money);
            Console.WriteLine(srednia.ToString("n4"));

            float odchylenie = KursWalut.LiczOdchylenieStandardowe(nowykurs._money, srednia);
            Console.WriteLine(odchylenie.ToString("n4"));

            float kursminimalny = KursWalut.KursMinimalny(nowykurs._money);
            Console.WriteLine(kursminimalny.ToString("n4"));

            float kursmaksymalny = KursWalut.KursMaksymalny(nowykurs._money);
            Console.WriteLine(kursmaksymalny.ToString("n4"));
            


            Console.WriteLine("FINISHED");
            Console.ReadKey();
        }
        
    }
}
