using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

namespace KursWalutDLL
{
    public class KursWalut
    {
        public static List<DateTime> dates = new List<DateTime>();
        public static Dictionary<DateTime, float> _money = new Dictionary<DateTime, float>();
        public static void SetDatesList(DateTime startDate, DateTime endDate)
        {

            for (var dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }
        }
        public static void Initialize(string moneycode)
        {
            Uri url = new Uri("https://www.nbp.pl/kursy/xml/dir" + dates[0].ToString("yyyy") + ".txt");
            HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(@"application/xml"));
            Console.WriteLine("https://www.nbp.pl/kursy/xml/dir" + dates[0].ToString("yyyy") + ".txt");
            var result = client.GetStringAsync(url).Result;
            //Console.WriteLine(result.ToString());

            string[] datelines = result.Split("\n");
            List<string> files = new List<string>();
            foreach (DateTime date in dates)
            {
                foreach (string line in datelines.Where(l => l.Contains(date.ToString("yyMMdd")) && l.StartsWith("a")))
                    files.Add(RemoveSpecialCharacters(line));
            }
            
            foreach (string t in files)
            {

                url = new Uri("http://www.nbp.pl/kursy/xml/" + RemoveSpecialCharacters(t) + ".xml");
                client = new HttpClient();
                result = client.GetStringAsync(url).Result;
                XDocument xdoc = XDocument.Parse(result);
                //Console.WriteLine(xdoc);
                foreach (XElement RateElement in xdoc.Element("tabela_kursow").Elements("pozycja"))
                {
                    if (RateElement.Element("kod_waluty").Value == moneycode)
                    {
                        _money.Add(DateTime.Parse(xdoc.Element("tabela_kursow").Element("data_publikacji").Value), float.Parse(RateElement.Element("kurs_sredni").Value.Replace(",", ".")));

                    }
                    //Console.WriteLine(RateElement.Element("kurs_sredni").Value);

                }
            }
        }
        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static float ObliczSrednia(Dictionary<DateTime, float> money)
        {

            float sum = 0;
            foreach (var t in money)
            {
                sum += t.Value;
            }
            return sum / money.Count;

        }
        public static float LiczOdchylenieStandardowe(Dictionary<DateTime, float> money, float srednia)
        {
            int i = 0;
            float sum = 0;
            foreach (var t in money)
            {
                i++;
                sum += (float)Math.Pow(t.Value - srednia, 2);

            }
            return sum / i;
        }
        public static float KursMinimalny(Dictionary<DateTime, float> money)
        {
            float? min = null;
            foreach (var t in money)
            {
                if (min == null)
                {
                    min = t.Value;
                }
                else
                {
                    if (min > t.Value)
                    {
                        min = t.Value;
                    }
                }

            }
            return (float)min;
        }
        public static float KursMaksymalny(Dictionary<DateTime, float> money)
        {
            float? max = null;
            foreach (var t in money)
            {
                if (max == null)
                {
                    max = t.Value;
                }
                else
                {
                    if (max < t.Value)
                    {
                        max = t.Value;
                    }
                }

            }
            return (float)max;
        }

    }
}
