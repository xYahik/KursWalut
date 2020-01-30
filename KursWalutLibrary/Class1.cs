using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KursWalutLibrary
{
    public class KursWalut
    {
        public class Kurs
        {
            public float Kurs_Kupna;
            public float Kurs_Sprzedarzy;
        }
        private List<DateTime> dates = new List<DateTime>();
        public Dictionary<DateTime, Kurs> _money = new Dictionary<DateTime, Kurs>();
        List<DateTime> Years = new List<DateTime>();
        public void SetDatesList(DateTime startDate, DateTime endDate)
        {

            for (var dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            
            for (var dt = startDate; dt <= endDate; dt = dt.AddYears(1))
            {
                Years.Add(dt);
            }
        }

        public void Initialize(string moneycode)
        {
            foreach (DateTime Year in Years)
            {
                Uri url = new Uri("https://www.nbp.pl/kursy/xml/dir" + Year.ToString("yyyy") + ".txt");
                HttpClient client = new HttpClient();
                var result = client.GetStringAsync(url).Result;

                string[] datelines = result.Split('\n');
                List<string> files = new List<string>();
                foreach (DateTime date in dates)
                {
                    foreach (string line in datelines.Where(l => l.Contains(date.ToString("yyMMdd")) && l.StartsWith("c")))
                        files.Add(line);
                }

                foreach (string t in files)
                {

                    url = new Uri("http://www.nbp.pl/kursy/xml/" + RemoveSpecialCharacters(t) + ".xml");
                    client = new HttpClient();
                    result = client.GetStringAsync(url).Result;
                    XDocument xdoc = XDocument.Parse(result);
                    foreach (XElement RateElement in xdoc.Element("tabela_kursow").Elements("pozycja"))
                    {
                        if (RateElement.Element("kod_waluty").Value == moneycode)
                        {
                            _money.Add(DateTime.Parse(xdoc.Element("tabela_kursow").Element("data_publikacji").Value), new Kurs { Kurs_Kupna = float.Parse(RateElement.Element("kurs_kupna").Value.Replace(",", ".")), Kurs_Sprzedarzy = float.Parse(RateElement.Element("kurs_sprzedazy").Value.Replace(",", ".")) });

                        }

                    }
                }
            }
        }
        private string RemoveSpecialCharacters(string str)
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

        public static float ObliczSrednia(Dictionary<DateTime, Kurs> money)
        {

            float sum = 0;
            foreach (var t in money)
            {
                sum += (t.Value.Kurs_Kupna+t.Value.Kurs_Sprzedarzy)/2;
            }
            return sum / money.Count;

        }
        public static float LiczOdchylenieStandardowe(Dictionary<DateTime, Kurs> money, float srednia)
        {
            int i = 0;
            float sum = 0;
            foreach (var t in money)
            {
                i++;
                sum += (float)Math.Pow((t.Value.Kurs_Kupna + t.Value.Kurs_Sprzedarzy) / 2 - srednia, 2);

            }
            return sum / i;
        }
        public static float KursMinimalny(Dictionary<DateTime, Kurs> money)
        {
            float? min = null;
            foreach (var t in money)
            {
                if (min == null)
                {
                    min = (t.Value.Kurs_Kupna + t.Value.Kurs_Sprzedarzy) / 2;
                }
                else
                {
                    if (min > (t.Value.Kurs_Kupna + t.Value.Kurs_Sprzedarzy) / 2)
                    {
                        min = (t.Value.Kurs_Kupna + t.Value.Kurs_Sprzedarzy) / 2;
                    }
                }

            }
            return (float)min;
        }
        public static float KursMaksymalny(Dictionary<DateTime, Kurs> money)
        {
            float? max = null;
            foreach (var t in money)
            {
                if (max == null)
                {
                    max = (t.Value.Kurs_Kupna + t.Value.Kurs_Sprzedarzy) / 2;
                }
                else
                {
                    if (max < (t.Value.Kurs_Kupna + t.Value.Kurs_Sprzedarzy) / 2)
                    {
                        max = (t.Value.Kurs_Kupna + t.Value.Kurs_Sprzedarzy) / 2;
                    }
                }

            }
            return (float)max;
        }

        public static Tuple<DateTime,float> RoznicaKursu(Dictionary<DateTime, Kurs> money)
        {

            float? roznica = null;
            DateTime data = DateTime.Now;
            foreach (var t in money)
            {
                if(roznica == null)
                {
                    roznica = Math.Abs(t.Value.Kurs_Kupna - t.Value.Kurs_Sprzedarzy);
                    data = t.Key;
                }
                else
                {
                    if (roznica < Math.Abs(t.Value.Kurs_Kupna - t.Value.Kurs_Sprzedarzy))
                    {
                        roznica = Math.Abs(t.Value.Kurs_Kupna - t.Value.Kurs_Sprzedarzy);
                        Console.WriteLine(t.Key);
                        data = t.Key;
                    }
                }
            }
            return Tuple.Create(data,(float)roznica);
        }

    }
}
