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

namespace ProjectBlya
{
    /// <summary>
    /// Логика взаимодействия для Card.xaml
    /// </summary>
    public partial class Card : UserControl
    {
        public Card()
        {
            InitializeComponent();
        }

        private void Expand_Click(object sender, RoutedEventArgs e)
        {
            if (hiddenLayout.Visibility == Visibility.Visible)
            {
                Expand_UnexpadneImage.Source = new BitmapImage(
                new Uri("pack://application:,,,/ProjectBlya;component/Expand_more.png"));

                hiddenLayout.Visibility = Visibility.Collapsed;
            }
            else {
                Expand_UnexpadneImage.Source = new BitmapImage(
                new Uri("pack://application:,,,/ProjectBlya;component/Expand_less.png"));

                hiddenLayout.Visibility = Visibility.Visible; 
            }
        }
    }
}
