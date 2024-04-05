using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PDSA_Games
{
    public class Solution
    {
        public int Number { get; set; }
        public int[,] Board { get; set; } 

        // Constructor to initialize the Board property
        public Solution(int number, int[,] board)
        {
            Number = number;
            Board = board;
        }
    }

    class Program
    {
        const int N = 8;

        public static List<Solution> GenerateSolutions()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<Solution> solutions = new List<Solution>();
            int count = 0;
            int[,] board = new int[N, N];

            // Initialize the board array to 0
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    board[i, j] = 0;
                }
            }

            // Initialize the pointer array
            int[] pointer = new int[N];
            for (int i = 0; i < N; i++)
            {
                pointer[i] = -1;
            }

            // Implementation of Back Tracking Algorithm
            for (int j = 0; ;)
            {
                pointer[j]++;

                // Reset and move one column back
                if (pointer[j] == N)
                {
                    board[pointer[j] - 1, j] = 0;
                    pointer[j] = -1;
                    j--;
                    if (j == -1)
                    {
                        break;
                    }
                }
                else
                {
                    board[pointer[j], j] = 1;
                    if (pointer[j] != 0)
                    {
                        board[pointer[j] - 1, j] = 0;
                    }
                    if (SolutionCheck(board))
                    {
                        j++; // move to next column
                        if (j == N)
                        {
                            j--;
                            count++;

                            // Create a copy of the board array to avoid modifying the same instance in solutions list
                            int[,] boardCopy = new int[N, N];
                            Array.Copy(board, boardCopy, board.Length);

                            solutions.Add(new Solution(count, boardCopy));
                        }
                    }
                }
            }
            stopwatch.Stop(); // Stop measuring time
            Trace.WriteLine("Normal time: " + stopwatch.ElapsedMilliseconds);
            return solutions;
        }

        public static bool SolutionCheck(int[,] board)
        {
            // Row check
            for (int i = 0; i < N; i++)
            {
                int sum = 0;
                for (int j = 0; j < N; j++)
                {
                    sum = sum + board[i, j];
                }
                if (sum > 1)
                {
                    return false;
                }
            }

            // Main diagonal check
            // above
            for (int i = 0, j = N - 2; j >= 0; j--)
            {
                int sum = 0;
                for (int p = i, q = j; q < N; p++, q++)
                {
                    sum = sum + board[p, q];
                }
                if (sum > 1)
                {
                    return false;
                }
            }

            // below
            for (int i = 1, j = 0; i < N - 1; i++)
            {
                int sum = 0;
                for (int p = i, q = j; p < N; p++, q++)
                {
                    sum = sum + board[p, q];
                }
                if (sum > 1)
                {
                    return false;
                }
            }

            // Minor diagonal check
            // above
            for (int i = 0, j = 1; j < N; j++)
            {
                int sum = 0;
                for (int p = i, q = j; q >= 0; p++, q--)
                {
                    sum = sum + board[p, q];
                }
                if (sum > 1)
                {
                    return false;
                }
            }

            // below
            for (int i = 1, j = N - 1; i < N - 1; i++)
            {
                int sum = 0;
                for (int p = i, q = j; p < N; p++, q--)
                {
                    sum = sum + board[p, q];
                }
                if (sum > 1)
                {
                    return false;
                }
            }
            return true;
        }
        public static int GetSolutionIndex(int[,] chessboard)
        {
            // Loop through the solutions list to find the index of the matching solution
            List<Solution> solutions = GenerateSolutions();
            int index = -1;

            foreach (Solution solution in solutions)
            {
                bool isMatch = true;

                // Compare the chessboard with the solution's board
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        if (chessboard[i, j] != solution.Board[i, j])
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (!isMatch)
                    {
                        break;
                    }
                }

                if (isMatch)
                {
                    index = solution.Number;
                    break;
                }
            }

            return index;
        }
    }
}