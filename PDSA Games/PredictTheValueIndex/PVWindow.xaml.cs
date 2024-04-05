using System;
using System.Collections.Generic;
using System.Windows;

namespace PDSA_Games
{
    public partial class PVWindow : Window
    {
        private readonly Random random = new Random();
        private readonly List<int> numbers = new List<int>();
        private int randomNumber;
        private readonly List<string> choices = new List<string> { "A", "B", "C", "D" };

        public List<string> Choices => choices;

        public PVWindow()
        {
            InitializeComponent();
            PredictionComboBox.ItemsSource = Choices;
        }

        private void StartNewGame_Click(object sender, RoutedEventArgs e)
        {
            numbers.Clear();
            for (int i = 0; i < 5000; i++)
            {
                numbers.Add(random.Next(1, 1000001));
            }
            randomNumber = numbers[random.Next(numbers.Count)];
            RandomNumberTextBlock.Text = $"Random Number: {randomNumber}";
        }

        private void SubmitPrediction_Click(object sender, RoutedEventArgs e)
        {
            string selectedChoice = PredictionComboBox.SelectedItem as string;
            if (selectedChoice != null)
            {
                int selectedIndex = choices.IndexOf(selectedChoice);
                int binaryIndex = SearchAlgorithms.BinarySearch(numbers, randomNumber);
                int jumpIndex = SearchAlgorithms.JumpSearch(numbers, randomNumber);
                int exponentialIndex = SearchAlgorithms.ExponentialSearch(numbers, randomNumber);
                int fibonacciIndex = SearchAlgorithms.FibonacciSearch(numbers, randomNumber);

                if (selectedIndex == binaryIndex && selectedIndex == jumpIndex && selectedIndex == exponentialIndex && selectedIndex == fibonacciIndex)
                {
                    ResultTextBlock.Text = "Correct Prediction!";
                    // Save the correct prediction to the database
                }
                else
                {
                    ResultTextBlock.Text = "Incorrect Prediction!";
                }
            }
        }
    }
}
