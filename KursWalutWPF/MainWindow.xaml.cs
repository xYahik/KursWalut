using System;
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
using KursWalutLibrary;
namespace KursWalutWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Licz_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDate = DataPoczatkowa.DisplayDate;
            DateTime endDate = DataKoncowa.DisplayDate;
            string kodwaluty = KodWaluty.Text;
            KursWalut nowykurs = new KursWalut();
            
            nowykurs.SetDatesList(startDate, endDate);
            nowykurs.Initialize(kodwaluty);


            float srednia = KursWalut.ObliczSrednia(nowykurs._money);
            SredniKurs.Content = srednia.ToString("n4");

            float odchylenie = KursWalut.LiczOdchylenieStandardowe(nowykurs._money, srednia);
            Odchylenie.Content = odchylenie.ToString("n4");

            float kursminimalny = KursWalut.KursMinimalny(nowykurs._money);
            KursMIN.Content = kursminimalny.ToString("n4");

            float kursmaksymalny = KursWalut.KursMaksymalny(nowykurs._money);
            KursMAX.Content = kursmaksymalny.ToString("n4");
        }
    }
}
