using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PDSA_Games
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void _8queens_Click(object sender, RoutedEventArgs e)
        {
            QueensPuzzle queensPuzzle = new QueensPuzzle();

            queensPuzzle.Show();

            Window mainWindow = Application.Current.MainWindow;
            mainWindow.Close();
        }

        private void tictactoe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void remValue_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
