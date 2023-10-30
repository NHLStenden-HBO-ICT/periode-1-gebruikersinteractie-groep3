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
    /// Interaction logic for GameOverScherm.xaml
    /// </summary>
    public partial class GameOverScherm : Window
    {
        public GameOverScherm()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
        }

        private void SpeelOpnieuwClick(object sender, RoutedEventArgs e)
        {
            GameWindow window = new GameWindow();
            window.Show();
            Close();
        }

        private void HoofdmenuClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
