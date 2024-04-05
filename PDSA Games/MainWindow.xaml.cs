using System.Security.Policy;
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
            DialogBoxTicTacToe ticTacToe = new DialogBoxTicTacToe();
            ticTacToe.Show();

            Window mainWindow = Application.Current.MainWindow;
            mainWindow.Close();

        }

        private void remValue_Click(object sender, RoutedEventArgs e)
        {
            RememberTheValue rememberTheValue = new RememberTheValue();
            rememberTheValue.Show();

            Window mainWindow = Application.Current.MainWindow;
            mainWindow.Close();
        }

        private void ShortestPath_Click(object sender, RoutedEventArgs e)
        {
            ShortestPath shortestPath = new ShortestPath();
            shortestPath.Show();
            Window mainWindow = Application.Current.MainWindow;
            mainWindow.Close();
        }

        private void PredictValue_Click(object sender, RoutedEventArgs e)
        {
            PVWindow pvWindow = new PVWindow();
            pvWindow.Show();
            Window mainWindow = Application.Current.MainWindow;
            mainWindow.Close();
        }
    }
}
