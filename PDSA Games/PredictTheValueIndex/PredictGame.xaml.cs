using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PDSA_Games
{
    public partial class PredictGame : Window
    {
        Random random = new Random();
        List<int> randomNumbers = new List<int>();
        List<int> Binaryindex = new List<int>();
        List<int> Jumpindex = new List<int>();
        List<int> Exponentialindex = new List<int>();
        List<int> Fibonacciindex = new List<int>();
        public int targetIndex;
        public int targetValue;
        public string Username { get; set; }


        public PredictGame()
        {
            InitializeComponent();
        }
       
        public void StartNewRound_Click(object sender, RoutedEventArgs e)
        {
            randomNumbers.Clear();
            for (int i = 0; i < 5000; i++)
            {
                randomNumbers.Add(random.Next(1, 1000000)); 
            }
            randomNumbers.Sort();
            targetIndex = random.Next(0, randomNumbers.Count);
            targetValue = randomNumbers[targetIndex];
            ResultsTextBlock.Text = "Random numbers generated.";
            lblVallue.Content = $"Find the index of: {targetValue}";

            List<int> comboBoxIndexes = new List<int>();
            comboBoxIndexes.Add(targetIndex); // Ensure target index is included

            while (comboBoxIndexes.Count < 4)
            {
                int randomIndex = random.Next(randomNumbers.Count);
                if (!comboBoxIndexes.Contains(randomIndex))
                {
                    comboBoxIndexes.Add(randomIndex);
                }
            }

            // Shuffle the indexes to randomize the ComboBox items
            comboBoxIndexes = Shuffle(comboBoxIndexes);

            // Set ComboBox items
            PredictionComboBox.Items.Clear();
            foreach (int index in comboBoxIndexes)
            {
                PredictionComboBox.Items.Add(new ComboBoxItem { Content = $"Option {index}" });
            }

            PredictionComboBox.IsEnabled = true;
            PredictionResultTextBlock.Text = "New round started. Predict the index of the target value.";
        }

        private void PerformSearches_Click(object sender, RoutedEventArgs e)
        {
            
            Stopwatch stopwatch = new Stopwatch();
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");


            // Perform binary search
            stopwatch.Start();
            for (int i = 1; i < 1000000; i++)
            {
                 Binaryindex.Add(BinarySearch(randomNumbers, i));
            }
            stopwatch.Stop();
            TimeSpan binarySearchTime = stopwatch.Elapsed;

            Binaryindex.RemoveAll(item => item == -1);
            string Binaryjson = JsonSerializer.Serialize(Binaryindex);

            using (var connection = new SqlConnection(connectionString))
                {
                    string insertQuery = "INSERT INTO PredictValueIndex (SearchMethod,IndexValues,TimeTaken) VALUES ('Binary Search',@Index,@Time)";
                    
                    connection.Execute(insertQuery, new {Index = Binaryjson, Time= binarySearchTime });
                }

                        
            // Perform Jump Search
            stopwatch.Restart();
            for (int i = 1; i < 1000000; i++)
            {
                Jumpindex.Add(JumpSearch(randomNumbers, i));
            }
            stopwatch.Stop();
            TimeSpan jumpSearchTime = stopwatch.Elapsed;
            Jumpindex.RemoveAll(item => item == -1);
            string Jumpjson = JsonSerializer.Serialize(Jumpindex);
            using (var connection = new SqlConnection(connectionString))
            {
                string insertQuery = "INSERT INTO PredictValueIndex (SearchMethod,IndexValues,TimeTaken) VALUES ('Jump Search',@Index,@Time)";

                connection.Execute(insertQuery, new { Index = Jumpjson, Time = jumpSearchTime });
            }

            // Perform Exponential Search
            stopwatch.Restart();
            for (int i = 1; i < 1000000; i++)
            {
                Exponentialindex.Add(ExponentialSearch(randomNumbers, i));
            }
            stopwatch.Stop();
            TimeSpan exponentialSearchTime = stopwatch.Elapsed;
            Exponentialindex.RemoveAll(item => item == -1);
            string Expojson = JsonSerializer.Serialize(Exponentialindex);
            using (var connection = new SqlConnection(connectionString))
            {
                string insertQuery = "INSERT INTO PredictValueIndex (SearchMethod,IndexValues,TimeTaken) VALUES ('Exponential Search',@Index,@Time)";

                connection.Execute(insertQuery, new { Index = Expojson, Time = exponentialSearchTime });
            }

            // Perform Fibonacci Search

            stopwatch.Restart();
            for (int i = 1; i < 1000000; i++)
            {
               Fibonacciindex.Add(FibonacciSearch(randomNumbers, i));

            }
            stopwatch.Stop();
            TimeSpan fibonacciSearchTime = stopwatch.Elapsed;
            Fibonacciindex.RemoveAll(item => item == -1);
            string Fibojson = JsonSerializer.Serialize(Fibonacciindex);
            using (var connection = new SqlConnection(connectionString))
            {
                string insertQuery = "INSERT INTO PredictValueIndex (SearchMethod,IndexValues,TimeTaken) VALUES ('Fibonacci Search',@Index,@Time)";

                connection.Execute(insertQuery, new { Index = Fibojson, Time = fibonacciSearchTime });
            }
            // Record results to database (not implemented here)

            ResultsTextBlock.Text = $"Search results:\n" +
                $"Binary Search - Time: {binarySearchTime}\n" +
                $"Jump Search - Time: {jumpSearchTime}\n" +
                $"Exponential Search - Time: {exponentialSearchTime}\n" +
                $"Fibonacci Search - Time: {fibonacciSearchTime}";
        }


        static int BinarySearch(List<int> list, int target)
        {
            int left = 0;
            int right = list.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (list[mid] == target)
                {
                    return mid; // Found the target, return the index
                }
                else if (list[mid] < target)
                {
                    left = mid + 1; // Continue searching in the right half
                }
                else
                {
                    right = mid - 1; // Continue searching in the left half
                }
            }

            return -1; // Target not found, return -1
        }
        static int JumpSearch(List<int> list, int target)
        {
            int n = list.Count;
            int step = (int)Math.Sqrt(n);
            int prev = 0;

            while (list[Math.Min(step, n) - 1] < target)
            {
                prev = step;
                step += (int)Math.Sqrt(n);
                if (prev >= n)
                {
                    return -1; // Target not found
                }
            }

            while (list[prev] < target)
            {
                prev++;

                if (prev == Math.Min(step, n))
                {
                    return -1; // Target not found
                }
            }

            if (list[prev] == target)
            {
                return prev; // Found the target
            }

            return -1; // Target not found
        }
        static int ExponentialSearch(List<int> list, int target)
        {
            if (list[0] == target)
            {
                return 0; // Target found at index 0
            }

            int i = 1;
            while (i < list.Count && list[i] <= target)
            {
                i *= 2; // Double the index for each iteration
            }

            return BinarySearch(list, target, i / 2, Math.Min(i, list.Count - 1));
        }
        static int BinarySearch(List<int> list, int target, int left, int right)
        {
            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (list[mid] == target)
                {
                    return mid; // Found the target
                }
                else if (list[mid] < target)
                {
                    left = mid + 1; // Continue searching in the right half
                }
                else
                {
                    right = mid - 1; // Continue searching in the left half
                }
            }

            return -1; // Target not found
        }
        static int FibonacciSearch(List<int> list, int target)
        {
            int fibMinus2 = 0; // Fibonacci number at index -2
            int fibMinus1 = 1; // Fibonacci number at index -1
            int fib = fibMinus1 + fibMinus2; // Current Fibonacci number

            // Find the smallest Fibonacci number greater than or equal to list.Count
            while (fib < list.Count)
            {
                fibMinus2 = fibMinus1;
                fibMinus1 = fib;
                fib = fibMinus1 + fibMinus2;
            }

            int offset = -1; // Offset from the beginning of the array

            while (fib > 1)
            {
                int i = Math.Min(offset + fibMinus2, list.Count - 1);

                if (list[i] < target)
                {
                    fib = fibMinus1;
                    fibMinus1 = fibMinus2;
                    fibMinus2 = fib - fibMinus1;
                    offset = i;
                }
                else if (list[i] > target)
                {
                    fib = fibMinus2;
                    fibMinus1 = fibMinus1 - fibMinus2;
                    fibMinus2 = fib - fibMinus1;
                }
                else
                {
                    return i; // Found the target
                }
            }

            // If the target is found at offset + 1
            if (fibMinus1 == 1 && offset + 1 < list.Count && list[offset + 1] == target)
            {
                return offset + 1;
            }

            return -1; // Target not found
        }

        private List<T> Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
        private void SubmitPrediction_Click(object sender, RoutedEventArgs e)
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            
            if (PredictionComboBox.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)PredictionComboBox.SelectedItem;
                int predictedIndex = int.Parse(selectedItem.Content.ToString().Split(' ')[1]);

                if (predictedIndex == targetIndex)
                {
                    PredictionResultTextBlock.Text = "Congratulations! Your prediction was correct.";

                    using (var connection = new SqlConnection(connectionString))
                    {
                        string insertQuery = "INSERT INTO PredictValueIndexGame (Username, IndexValues) VALUES (@Username,@Index)";

                        connection.Execute(insertQuery, new { Username = Username, Index = targetIndex });
                    }
                }
                else
                {
                    PredictionResultTextBlock.Text = $"Incorrect. The correct index is {targetIndex}.";
                }

                PredictionComboBox.IsEnabled = false;
            }
            else
            {
                PredictionResultTextBlock.Text = "Please select a prediction before submitting.";
            }
        }

        private void PredictionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PredictionResultTextBlock.Text = ""; // Clear previous prediction result
        }
    }
}
