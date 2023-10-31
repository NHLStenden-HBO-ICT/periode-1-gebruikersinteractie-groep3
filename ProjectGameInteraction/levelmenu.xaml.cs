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
using System.Windows.Shapes;

namespace ProjectGameInteraction
{
    /// <summary>
    /// Interaction logic for levelmenu.xaml
    /// </summary>
    public partial class levelmenu : Window
    {
        public levelmenu()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
        }

        private void ReturnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Level1Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow();
            gameWindow.Show();
            Close();
        }

        private void Level2Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dit level is niet beschikbaar");
        }

        private void Level3Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dit level is niet beschikbaar");
        }

        private void Level4Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dit level is niet beschikbaar");
        }

        private void Level5Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dit level is niet beschikbaar");
        }

        private void Level6Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dit level is niet beschikbaar");
        }
    }
}
