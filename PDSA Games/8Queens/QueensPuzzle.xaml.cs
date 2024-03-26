using PDSA_Games._8Queens;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PDSA_Games
{
    /// <summary>
    /// Interaction logic for QueensPuzzle.xaml
    /// </summary>
    public partial class QueensPuzzle : Window
    {
        public QueensPuzzle()
        {
            InitializeComponent();
        }

        private void startGame_Click(object sender, RoutedEventArgs e)
        {

            Solutions solutions = new Solutions();
            QueensPuzzle queensPuzzle = new QueensPuzzle();

            solutions.Show();

            Window mainWindow = Application.Current.MainWindow;
            queensPuzzle.Close();

        }
    }
}
