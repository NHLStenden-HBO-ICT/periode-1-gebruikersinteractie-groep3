﻿using System;
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
    /// Interaction logic for ParentalControlTimerSet.xaml
    /// </summary>
    public partial class ParentalControlTimerSet : Window
    {
        private DispatcherTimer timer;
        private int timerDuration; // De duur van de timer in minuten

        public ParentalControlTimerSet()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
        }

        private void ReturnToParentalMenu_Click(object sender, RoutedEventArgs e)
        {
            parentalcontrolmenu pc = new parentalcontrolmenu();
            pc.Show();
            
            Close();
        }

        private void StartTimer_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(timerTextBox.Text, out int minutes) && minutes > 0 && minutes <= 60)
            {
                // Converteer de ingevoerde minuten naar seconden
                timerDuration = minutes * 60;

                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
            }
            else
            {
                MessageBox.Show("Voer een geldige timerduur in (positieve gehele getallen minder dan of gelijk aan 60 minuten).");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timerDuration > 0)
            {
                timerDuration--;
                UpdateTimerDisplay();
            }
            else
            {
                MessageBox.Show("Hey speler! Het is tijd om een gamepauze te nemen" + Environment.NewLine + "We kunnen later weer spelen! ", "Speeltijd verlopen!", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
                // Timer is afgelopen, sluit de applicatie

            }
        }

        private void ResetTimer_Click(object sender, RoutedEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                timerDuration = 0; // Stel de timerduur op nul in om te resetten
                timerTextBox.Text = ""; // Leeg de TextBox voor de timerduur
            }
        }

        private void UpdateTimerDisplay()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(timerDuration);
            timerDisplay.Text = timeSpan.ToString(@"mm\:ss");
        }
    }
}
