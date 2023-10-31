using System;
using System.Collections.Generic;
using System.IO;
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
            if (!File.Exists("pincode.txt")) File.Create("pincode.txt");
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            mediaPlayer.Open(new Uri("Afbeeldingen\\music2.mp3", UriKind.Relative));
            mediaPlayer.MediaEnded += new EventHandler(Media_Ended);
            mediaPlayer.Play();

            if (Properties.Settings.Default.setting)
            {
                mediaPlayer.Play();
            }
        }

        private void Media_Ended(object? sender, EventArgs e)
        {
            if (Properties.Settings.Default.setting)
            {
                mediaPlayer.Position = TimeSpan.Zero;
                mediaPlayer.Play();
            }
        }


        // PC -> Parental Control
        private void PCClick(object sender, RoutedEventArgs e)
        {
            parentalcontrolmenu pc = new parentalcontrolmenu();
            pc.Show();
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            
            settings ws = new settings();
            {
                Owner = this.Parent as Window;
                ShowInTaskbar = false;
            }
            ws.ShowDialog();
            
            if ((bool)Properties.Settings.Default.setting == false)
            {
                mediaPlayer.Stop();
            }
            else
            {
                mediaPlayer.Position = TimeSpan.Zero;
                mediaPlayer.Play();
            }
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if ((bool)Properties.Settings.Default.setting == false)
            {
                mediaPlayer.Stop();
            }
            else
            {
                mediaPlayer.Position = TimeSpan.Zero;
                mediaPlayer.Play();
            }
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
