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
using System.Windows.Threading;

namespace ProjectGameInteraction
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        DispatcherTimer SplashTimer = new DispatcherTimer();
        private const int SPLASHTIME = 1;
        public SplashScreen()

        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            // level timer
            SplashTimer.Interval = TimeSpan.FromSeconds(SPLASHTIME);
            SplashTimer.Tick += SplashTick;
            SplashTimer.Start();
            splashTime = SPLASHTIME;


        }

        private int splashTime = 0;
        private void SplashTick(object? sender, EventArgs e)
        {
            if (splashTime == 0)
            {
                MainWindow MainWindow = new();
                MainWindow.Show();
                Close();
                SplashTimer.Stop();
            }
            else
                splashTime--;




        }
    }

}

