using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for settings.xaml
    /// </summary>
    public partial class settings : Window, INotifyPropertyChanged, IDisposable
    {
        void IDisposable.Dispose() { }
        private double _volume;
        private bool mouseCaptured = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MusicBox.IsChecked = Properties.Settings.Default.setting;
        }


        public double Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                OnPropertyChanged("Volume");
            }
        }

        public settings()
        {
            InitializeComponent();
            DataContext = this;
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
        }

        private new void MouseMove(object? sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && mouseCaptured)
            {
                var x = e.GetPosition(volumeBar).X;
                var ratio = x / volumeBar.ActualWidth;
                Volume = ratio * volumeBar.Maximum;
            }
        }

        private new void MouseDown(object? sender, MouseButtonEventArgs e)
        {
            mouseCaptured = true;
            var x = e.GetPosition(volumeBar).X;
            var ratio = x / volumeBar.ActualWidth;
            Volume = ratio * volumeBar.Maximum;
        }

        private new void MouseUp(object? sender, MouseButtonEventArgs e)
        {
            mouseCaptured = false;
        }

        #region Property Changed

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void ReturnClick(object? sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.setting = MusicBox.IsChecked.GetValueOrDefault();
            Properties.Settings.Default.Save();
            Close();
        }
    }
}
