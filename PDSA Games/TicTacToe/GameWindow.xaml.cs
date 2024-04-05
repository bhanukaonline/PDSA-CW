using Dapper;
using Microsoft.Data.SqlClient;
using PDSA_Games;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PDSA_Games
{
    public partial class GameWindow : Window
    {
        private TicTacToeGame game;
        private Button[,] buttons;
        public string Username { get; set; }

        public GameWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            game = new TicTacToeGame();
            buttons = new Button[3, 3] { { btn00, btn01, btn02 }, { btn10, btn11, btn12 }, { btn20, btn21, btn22 } };
            UpdateBoardUI();
        }

        private void UpdateBoardUI()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    buttons[row, col].Content = game.GetBoardCell(row, col);
                }
            }

            string winner = game.CheckWinner();
            if (!string.IsNullOrEmpty(winner))
            {

                if (winner == "O")
                {
                    MessageBox.Show("Computer Won :(");

                }
                string boardString = "";
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    boardString += game.GetBoardCell(row, col);
                }
                    
            }
                if (winner == "X")
                {
                    MessageBox.Show("Congratulations! You won!");
                    var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

                    using (var connection = new SqlConnection(connectionString))
                    {
                        string insertQuery = " insert into TicTacToe (Name,WinningBoard) values( @Username,@boardString)";


                        connection.Execute(insertQuery, new { Username = Username, boardString = boardString });


                    }
                }
                InitializeGame();
            }
            else if (game.IsBoardFull())
            {
                MessageBox.Show("It's a draw!");
                InitializeGame();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            if (game.MakeMove(row, col))
            {
                UpdateBoardUI();
                if (game.CurrentPlayer == 'O' && !game.IsBoardFull() && string.IsNullOrEmpty(game.CheckWinner()))
                {
                    // Computer's turn (O)
                    ComputerMove();
                    UpdateBoardUI();
                }
            }
            else
            {
                MessageBox.Show("Invalid move! Try again.");
            }
        }

        private void ComputerMove()
        {
            // Implement the Minimax algorithm with Alpha-Beta pruning for the computer player
            // This part of the code will determine the best move for the computer player
            // You can refer to online resources or tutorials on Minimax with Alpha-Beta pruning implementation in C#
            // Here's a simplified version for demonstration purposes:

            int bestScore = int.MinValue;
            int bestRow = -1;
            int bestCol = -1;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (game.IsCellEmpty(row, col))
                    {
                        game.MakeMove(row, col); // Simulate the move
                        int score = Minimax(0, false); // Calculate score using Minimax
                        game.UndoMove(row, col); // Undo the move

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestRow = row;
                            bestCol = col;
                        }
                    }
                }
            }

            game.MakeMove(bestRow, bestCol); // Make the best move for the computer player
        }

        private int Minimax(int depth, bool isMaximizingPlayer)
        {
            string winner = game.CheckWinner();
            if (!string.IsNullOrEmpty(winner))
            {
                return winner == "X" ? -1 : 1; // Return score based on the winner
            }

            if (game.IsBoardFull())
            {
                return 0; // Return 0 for a draw
            }

            if (isMaximizingPlayer)
            {
                int bestScore = int.MinValue;
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (game.IsCellEmpty(row, col))
                        {
                            game.MakeMove(row, col); // Simulate the move
                            int score = Minimax(depth + 1, false); // Recursive call for the next level
                            game.UndoMove(row, col); // Undo the move
                            bestScore = Math.Max(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (game.IsCellEmpty(row, col))
                        {
                            game.MakeMove(row, col); // Simulate the move
                            int score = Minimax(depth + 1, true); // Recursive call for the next level
                            game.UndoMove(row, col); // Undo the move
                            bestScore = Math.Min(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
        }
    }
}
