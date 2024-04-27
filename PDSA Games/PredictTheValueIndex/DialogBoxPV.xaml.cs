using System;
using System.Collections.Generic;
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

namespace PDSA_Games
{
    /// <summary>
    /// Interaction logic for DialogBox.xaml
    /// </summary>
    public partial class DialogBoxPV : Window
    {
        
        public DialogBoxPV()
        {
            InitializeComponent();
            lblQuestion.Content = "Enter Your Name:";
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            string username = txtAnswer.Text;
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Username cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (username.Length < 3)
            {
                MessageBox.Show("Username must be at least 3 characters long.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (username.Length > 20)
            {
                MessageBox.Show("Username cannot exceed 20 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            {
                MessageBox.Show("Username can only contain letters, numbers, and underscores.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // If all validations pass, proceed with opening the new window
            PredictGame anotherWindow = new PredictGame();
            anotherWindow.Username = username;
            anotherWindow.Show();
            this.Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        public string Answer
        {
            get { return txtAnswer.Text; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
