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
    public partial class MainWindow : Window, IDisposable
    {
        void IDisposable.Dispose() { }

        private MediaPlayer mediaPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            mediaPlayer.Open(new Uri(string.Format("{0}\\music2.mp3", AppDomain.CurrentDomain.BaseDirectory)));
            mediaPlayer.MediaEnded += new EventHandler(Media_Ended);
            mediaPlayer.Play();
        }

        private void Media_Ended(object? sender, EventArgs e)
        {
            mediaPlayer.Position = TimeSpan.Zero;
            throw new NotImplementedException();
        }


        // PC -> Parental Control
        private void PCClick(object sender, RoutedEventArgs e)
        {
            parentalcontrolmenu pc = new parentalcontrolmenu();
            pc.Show();
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            settings set = new settings();
            set.Show();
        }

        private void ShopClick(object sender, RoutedEventArgs e)
        {
            shop sh = new shop();
            sh.Show();
        }

        private void PlayClick(object sender, RoutedEventArgs e)
        {
            levelmenu lm = new levelmenu();
            lm.Show();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        
    }
}
