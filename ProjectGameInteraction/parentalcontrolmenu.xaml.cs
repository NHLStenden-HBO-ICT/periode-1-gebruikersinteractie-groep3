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
    /// Interaction logic for parentalcontrolmenu.xaml
    /// </summary>
    public partial class parentalcontrolmenu : Window
    {
        
        public parentalcontrolmenu()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
        }

        private void ReturnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private string savedPin = "";
        private void SetPinButton_Click(object sender, RoutedEventArgs e)
        {
            string newPin = PinInputTextBox.Text;

            if (!string.IsNullOrWhiteSpace(newPin))
            {
                savedPin = newPin;
                MessageBox.Show("Pincode ingesteld.");
                PinInputTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Voer een geldige pincode in.");
            }
        }

        private void UnlockButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredPin = EnteredPinTextBox.Text;

            if (enteredPin == savedPin)
            {
                ParentalControlTimerSet parentalTimerset = new ParentalControlTimerSet();
                parentalTimerset.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Onjuiste pincode. Probeer opnieuw.");
                EnteredPinTextBox.Clear();
            }
        }

        private void PinInputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void EnteredPinTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}