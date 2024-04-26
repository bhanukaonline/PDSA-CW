using System;
using System.Collections.Generic;
using System.Data;
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

namespace PDSA_Games._8Queens
{
    /// <summary>
    /// Interaction logic for Solutions.xaml
    /// </summary>
    public partial class Solutions : Window
    {
        private List<Solution> solutions;
        private int currentSolutionIndex;

        public Solutions()
        {
            InitializeComponent();
            solutions = Program.GenerateSolutions();
            currentSolutionIndex = -1;
        }

        private DataTable ConvertToDataTable(int[,] array)
        {
            DataTable dataTable = new DataTable();
            for (int i = 0; i < array.GetLength(1); i++)
            {
                dataTable.Columns.Add(i.ToString());
            }

            for (int i = 0; i < array.GetLength(0); i++)
            {
                DataRow row = dataTable.NewRow();
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    row[j.ToString()] = array[i, j];
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (solutions.Any()) // Check if there are any solutions
            {
                currentSolutionIndex = (currentSolutionIndex + 1) % solutions.Count;
                var currentSolution = solutions[currentSolutionIndex];
                dataGrid.ItemsSource = ConvertToDataTable(currentSolution.Board).DefaultView;
                lblSolution.Content = $"Solution {currentSolutionIndex + 1} of {solutions.Count}";
            }
        }
    }
}
