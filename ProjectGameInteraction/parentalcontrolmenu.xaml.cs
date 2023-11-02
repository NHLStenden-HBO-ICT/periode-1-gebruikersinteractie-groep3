using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for parentalcontrolmenu.xaml
    /// </summary>
    public partial class parentalcontrolmenu : Window
    {
        private string savedPin;

        public parentalcontrolmenu()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            savedPin = string.Empty;
            string? line;
            try
            {
                StreamReader sr = new("pincode.txt");
                line = sr.ReadLine();
                while (line != null)
                {
                    if (line.StartsWith("pin:"))
                    {
                        savedPin = line.Split(':')[1].TrimEnd();
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            PinInputTextBox.Focus();
        }

        private void ReturnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void IsNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SetPinButton_Click(object sender, RoutedEventArgs e)
        {
            if (PinInputTextBox.Text.Length != 4)
            {
                MessageBox.Show("Voer een geldige pincode in.");
                PinInputTextBox.Clear();
                PinInputTextBox.Focus();
                return;
            }
            if (savedPin != string.Empty)
            {
                MessageBox.Show("Pincode is al ingesteld.");
                PinInputTextBox.Clear();
                PinInputTextBox.Focus();
                return;
            }

            try
            {
                StreamWriter sw = new StreamWriter("pincode.txt");
                sw.WriteLine("pin:" + PinInputTextBox.Text);
                sw.Close();
                MessageBox.Show("Pincode ingesteld.");
                PinInputTextBox.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private void UnlockButton_Click(object sender, RoutedEventArgs e)
        {
            if (savedPin == string.Empty || PinInputTextBox.Text.Length != 4 || PinInputTextBox.Text != savedPin)
            {
                MessageBox.Show("Onjuiste pincode. Probeer opnieuw.");
                PinInputTextBox.Clear();
                PinInputTextBox.Focus();
                return;
            }
            ParentalControlTimerSet parentalTimerset = new ParentalControlTimerSet();
            parentalTimerset.Show();
            Close();
        }

        private void PinInputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void EnteredPinTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}