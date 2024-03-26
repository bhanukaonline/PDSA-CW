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
        public Solutions()
        {
            InitializeComponent();
            List<Solution> solutions = Program.GenerateSolutions();
            if (solutions.Any()) // Check if there are any solutions
            {
                var firstSolution = solutions.First();
                dataGrid.ItemsSource = ConvertToDataTable(firstSolution.Board).DefaultView;
            }
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

    }
}
