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

namespace ProjectGameInteraction
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
        }

        private void PCClick(object sender, RoutedEventArgs e)
        {
            parentalcontrolmenu pc = new parentalcontrolmenu();
            pc.Visibility = Visibility.Visible;

            this.Close();
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            settings set = new settings();
            set.Visibility = Visibility.Visible;

            this.Close();
        }

        private void ShopClick(object sender, RoutedEventArgs e)
        {
            shop sh = new shop();
            sh.Visibility = Visibility.Visible;

            this.Close();
        }

        private void PlayClick(object sender, RoutedEventArgs e)
        {
            levelmenu lm = new levelmenu();
            lm.Visibility = Visibility.Visible;

            this.Close();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
