public class TicTacToeGame
{
    private char[,] board; // Represents the Tic-Tac-Toe board
    private char currentPlayer;

    public TicTacToeGame()
    {
        board = new char[3, 3]; // Initialize the board
        currentPlayer = 'X'; // Start with player X
    }

    public char GetBoardCell(int row, int col)
    {
        return board[row, col];
    }

    public char CurrentPlayer => currentPlayer;

    public bool MakeMove(int row, int col)
    {
        if (board[row, col] == '\0') // Check if the cell is empty
        {
            board[row, col] = currentPlayer;
            currentPlayer = currentPlayer == 'X' ? 'O' : 'X'; // Switch player
            return true; // Move successful
        }
        return false; // Cell already occupied
    }

    public string CheckWinner()
    {
        // Check rows, columns, and diagonals for a winner
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != '\0')
                return board[i, 0].ToString(); // Row win

            if (board[0, i] == board[1, i] && board[1, i] == board[2, i] && board[0, i] != '\0')
                return board[0, i].ToString(); // Column win
        }

        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != '\0')
            return board[0, 0].ToString(); // Diagonal (top-left to bottom-right) win

        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != '\0')
            return board[0, 2].ToString(); // Diagonal (top-right to bottom-left) win

        return string.Empty; // No winner yet
    }

    public bool IsBoardFull()
    {
        // Check if the board is full (all cells occupied)
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (board[row, col] == '\0')
                    return false; // Board not full
            }
        }
        return true; // Board is full
    }


    public bool IsCellEmpty(int row, int col)
    {
        return board[row, col] == '\0';
    }

    public void UndoMove(int row, int col)
    {
        board[row, col] = '\0'; // Clear the cell
        currentPlayer = currentPlayer == 'X' ? 'O' : 'X'; // Switch back to previous player
    }
}
