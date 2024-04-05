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

namespace PDSA_Games
{
    /// <summary>
    /// Interaction logic for DialogBox.xaml
    /// </summary>
    public partial class DialogBoxRV : Window
    {
        
        public DialogBoxRV()
        {
            InitializeComponent();
            lblQuestion.Content = "Enter Your Name:";
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            string username = txtAnswer.Text;
            if (!string.IsNullOrEmpty(username))
            {
                RememberGame anotherWindow = new RememberGame();
                anotherWindow.Username = username;
                anotherWindow.Show();
                this.Close();


            }
            else
            {
                MessageBox.Show("Username cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
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
