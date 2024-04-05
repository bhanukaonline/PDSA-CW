using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
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

namespace PDSA_Games._8Queens
{
    /// <summary>
    /// Interaction logic for PlaySolution.xaml
    /// </summary>
    public partial class PlaySolution : Window
    {
        private Button[,] chessboardButtons = new Button[8, 8];
        private int[,] chessboard = new int[8, 8];

        public string Username { get; set; }
        public PlaySolution()
        {
            InitializeComponent();
            InitializeChessboard();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void InitializeChessboard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Button button = new Button();
                    button.Click += ChessboardButton_Click;
                    ChessboardGrid.Children.Add(button);
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                    chessboardButtons[row, col] = button; // Store button reference
                }
            }
        }

        private void ChessboardButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            int row = Grid.GetRow(clickedButton);
            int col = Grid.GetColumn(clickedButton);

            // Toggle the state of the square (1 if queen is placed, 0 if empty)
            chessboard[row, col] = (chessboard[row, col] == 0) ? 1 : 0;

            // Update UI based on the chessboard state
            UpdateChessboardUI();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Call SolutionCheck function and update result label


            int count = 0;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (chessboard[row, col] == 1)
                    {
                        count++;
                    }
                }
            }

            bool hasEightQueens = count == 8;

            if (hasEightQueens)
            {
                bool isSolutionCorrect = Program.SolutionCheck(chessboard);

                int? solutionFound;


                if (isSolutionCorrect)
                {

                    ResultLabel.Content = "Correct solution!";

                    int Index = Program.GetSolutionIndex(chessboard);
                    lblIndex.Content = "Your found solution Number: " + Index;
                    var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

                    using (var connection = new SqlConnection(connectionString))
                    {
                        string checkQuery = "select SolutionIndex from Winner where (SolutionIndex = @Index or SolutionIndex is Null)";


                        solutionFound = connection.Query<int?>(checkQuery, new { Index = Index }).FirstOrDefault();


                    }

                    if (solutionFound == null)
                    {
                        using (var connection = new SqlConnection(connectionString))
                        {
                            string insertQuery = "INSERT INTO Winner (Name, SolutionIndex) VALUES (@Value1, @Value2)";
                            string flagQuery = "SELECT COUNT(SolutionIndex)" +
                                             "FROM Winner";
                            string clearFlagQuery = "delete from Winner";
                            connection.Execute(insertQuery, new { Value1 = Username, Value2 = Index });

                            int? flag = connection.Query<int?>(flagQuery).FirstOrDefault();
                            if (flag == 92)
                            {
                                connection.Execute(flagQuery);
                            }
                        }
                    }
                    else
                    {
                        ResultLabel.Content = "Solution already found!. Try again";
                    }
                }
                else
                {
                    ResultLabel.Content = "Incorrect solution!";
                }

            }
            else
            {
                ResultLabel.Content = "You must place 8 queens on the board!";

            }


        }
        private void UpdateChessboardUI()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Button button = chessboardButtons[row, col];

                    // Change button appearance based on the chessboard state
                    if (chessboard[row, col] == 1)
                    {
                        button.Content = "Q"; // Display Q for placed queens
                    }
                    else
                    {
                        button.Content = "";


                    }
                }
            }
        }

        

        private void btnClear_Click_1(object sender, RoutedEventArgs e)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    chessboard[row, col] = 0;
                }
            }

            // Update the UI based on the cleared chessboard array
            UpdateChessboardUI();
        }
    }
}
