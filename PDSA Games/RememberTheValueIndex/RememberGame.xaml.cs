using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace PDSA_Games
{
    /// <summary>
    /// Interaction logic for RememberGame.xaml
    /// </summary>
    public partial class RememberGame : Window
    {
        Random random = new Random();
        public string Username { get; set; }
        List<int> randomNumbers = new List<int>();
        private List<int> sortedNumbers = new List<int>();

        public RememberGame()
        {
            InitializeComponent();
        }

        private void StartRound_Click(object sender, RoutedEventArgs e)
        {
            randomNumbers.Clear();
            sortedNumbers.Clear();

            for (int i = 0; i < 5000; i++)
            {
                randomNumbers.Add(random.Next(1, 1000000));
            }
            randomNumbers.Sort();
            sortedNumbers.AddRange(randomNumbers.GetRange(0, 20));
            ResultsTextBlock.Text = "New round started. Random numbers generated.";
            DisplayNumbersOneByOne();
        }
        private async void DisplayNumbersOneByOne()
        {
            string connectionString = (string)Application.Current.Resources["ConnectionString"];

            foreach (var number in sortedNumbers)
            {
                ResultsTextBlock.Text = $"Displaying number: {number}";
                await Task.Delay(2000); // Delay for 2 seconds
            }

            int firstRandomIndex = random.Next(0, 20);
            int secondRandomIndex;
            do
            {
                secondRandomIndex = random.Next(0, 20);
            } while (secondRandomIndex == firstRandomIndex);

            int firstRandomValue = sortedNumbers[firstRandomIndex];
            int secondRandomValue = sortedNumbers[secondRandomIndex];

            // Prompt user to enter the index of the two randomly chosen values
            int userInput1 = PromptUserForIndex(firstRandomValue);
            int userInput2 = PromptUserForIndex(secondRandomValue);

            // Check if the user identified the answer correctly
            bool isCorrect1 = userInput1.ToString() == firstRandomIndex.ToString();
            bool isCorrect2 = userInput2.ToString() == secondRandomIndex.ToString();

            if (isCorrect1 && isCorrect2)
            {
                ResultsTextBlock.Text = "Congratulations! You correctly identified both values.";
                using (var connection = new SqlConnection(connectionString))
                {
                    string insertQuery = "INSERT INTO RememberValueIndexGame (Username,FirstValue,FirstIndex,SecondValue,SecondIndex) VALUES (@Username,@FirstValue,@FirstIndex,@SecondValue,@SecondIndex)";
                    connection.Execute(insertQuery, new { Username, FirstValue = firstRandomValue, FirstIndex = firstRandomIndex, SecondValue = secondRandomValue, SecondIndex = secondRandomIndex});
                }

            }
            else
            {
                ResultsTextBlock.Text = "Sorry, you did not identify the values correctly.";
            }
        }

        private int PromptUserForIndex(int value)
        {
            int userInput;
            string inputString;
            do
            {
                inputString = Microsoft.VisualBasic.Interaction.InputBox($"Enter the index of {value}:");
            } while (!int.TryParse(inputString, out userInput) || userInput < 0 || userInput >= 20);

            return userInput;
        }

        private void PerformSort_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            string connectionString = (string)Application.Current.Resources["ConnectionString"];

            // Bubble Sort
            stopwatch.Start();
            List<int> Bubblesort = SortingAlgorithms.BubbleSort(randomNumbers);
            stopwatch.Stop();
            TimeSpan bubbleSortTime = stopwatch.Elapsed;
            using (var connection = new SqlConnection(connectionString))
            {
                string insertQuery = "INSERT INTO RememberValueIndex (SortingMethod,TimeTaken) VALUES ('Bubble Sort',@Time)";
                connection.Execute(insertQuery, new { Time = bubbleSortTime });
            }

            // Insertion Sort
            stopwatch.Restart();
            List<int> Insertionsort = SortingAlgorithms.InsertionSort(randomNumbers);
            stopwatch.Stop();
            TimeSpan insertionSortTime = stopwatch.Elapsed;
            using (var connection = new SqlConnection(connectionString))
            {
                string insertQuery = "INSERT INTO RememberValueIndex (SortingMethod,TimeTaken) VALUES ('Insertion Sort',@Time)";
                connection.Execute(insertQuery, new { Time = insertionSortTime });
            }

            // Merge Sort
            stopwatch.Restart();
            List<int> Mergesort = SortingAlgorithms.MergeSort(randomNumbers);
            stopwatch.Stop();
            TimeSpan mergeSortTime = stopwatch.Elapsed;
            using (var connection = new SqlConnection(connectionString))
            {
                string insertQuery = "INSERT INTO RememberValueIndex (SortingMethod,TimeTaken) VALUES ('Merge Sort',@Time)";
                connection.Execute(insertQuery, new { Time = mergeSortTime });
            }

            // Radix Sort
            stopwatch.Restart();
            List<int> Radixsort = SortingAlgorithms.RadixSort(randomNumbers);
            stopwatch.Stop();
            TimeSpan radixSortTime = stopwatch.Elapsed;
            using (var connection = new SqlConnection(connectionString))
            {
                string insertQuery = "INSERT INTO RememberValueIndex (SortingMethod,TimeTaken) VALUES ('Radix Sort',@Time)";
                connection.Execute(insertQuery, new { Time = radixSortTime });
            }

            // Shell Sort
            stopwatch.Restart();
            List<int> Shellsort = SortingAlgorithms.ShellSort(randomNumbers);
            stopwatch.Stop();
            TimeSpan shellSortTime = stopwatch.Elapsed;
            using (var connection = new SqlConnection(connectionString))
            {
                string insertQuery = "INSERT INTO RememberValueIndex (SortingMethod,TimeTaken) VALUES ('Shell Sort',@Time)";
                connection.Execute(insertQuery, new { Time = shellSortTime });
            }

            // Quick Sort
            stopwatch.Restart();
            List<int> Quicksort = SortingAlgorithms.QuickSort(randomNumbers);
            stopwatch.Stop();
            TimeSpan quickSortTime = stopwatch.Elapsed;
            using (var connection = new SqlConnection(connectionString))
            {
                string insertQuery = "INSERT INTO RememberValueIndex (SortingMethod,TimeTaken) VALUES ('Quick Sort',@Time)";
                connection.Execute(insertQuery, new { Time = quickSortTime });
            }

            // Tim Sort
            stopwatch.Restart();
            List<int> Timsort = SortingAlgorithms.TimSort(randomNumbers);
            stopwatch.Stop();
            TimeSpan timSortTime = stopwatch.Elapsed;
            using (var connection = new SqlConnection(connectionString))
            {
                string insertQuery = "INSERT INTO RememberValueIndex (SortingMethod,TimeTaken) VALUES ('Tim Sort',@Time)";
                connection.Execute(insertQuery, new { Time = timSortTime });
            }





        }
        public class SortingAlgorithms
        {
            public static List<int> BubbleSort(List<int> array)
            {
                List<int> sortedArray = new List<int>(array);

                int n = sortedArray.Count;
                for (int i = 0; i < n - 1; i++)
                {
                    for (int j = 0; j < n - i - 1; j++)
                    {
                        if (sortedArray[j] > sortedArray[j + 1])
                        {
                            int temp = sortedArray[j];
                            sortedArray[j] = sortedArray[j + 1];
                            sortedArray[j + 1] = temp;
                        }
                    }
                }

                return sortedArray;
            }

            public static List<int> InsertionSort(List<int> array)
            {
                List<int> sortedArray = new List<int>(array);

                int n = sortedArray.Count;
                for (int i = 1; i < n; ++i)
                {
                    int key = sortedArray[i];
                    int j = i - 1;

                    while (j >= 0 && sortedArray[j] > key)
                    {
                        sortedArray[j + 1] = sortedArray[j];
                        j = j - 1;
                    }
                    sortedArray[j + 1] = key;
                }

                return sortedArray;
            }

            public static List<int> MergeSort(List<int> array)
            {
                if (array.Count <= 1)
                    return new List<int>(array);

                List<int> left = new List<int>();
                List<int> right = new List<int>();

                int middle = array.Count / 2;
                for (int i = 0; i < middle; i++)
                    left.Add(array[i]);
                for (int i = middle; i < array.Count; i++)
                    right.Add(array[i]);

                left = MergeSort(left);
                right = MergeSort(right);

                return Merge(left, right);
            }

            private static List<int> Merge(List<int> left, List<int> right)
            {
                List<int> result = new List<int>();

                while (left.Count > 0 && right.Count > 0)
                {
                    if (left[0] <= right[0])
                    {
                        result.Add(left[0]);
                        left.RemoveAt(0);
                    }
                    else
                    {
                        result.Add(right[0]);
                        right.RemoveAt(0);
                    }
                }

                while (left.Count > 0)
                {
                    result.Add(left[0]);
                    left.RemoveAt(0);
                }

                while (right.Count > 0)
                {
                    result.Add(right[0]);
                    right.RemoveAt(0);
                }

                return result;
            }

            public static List<int> RadixSort(List<int> array)
            {
                int max = MaxValue(array);
                List<int> sortedArray = new List<int>(array);

                for (int exp = 1; max / exp > 0; exp *= 10)
                    CountSort(sortedArray, exp);

                return sortedArray;
            }

            private static int MaxValue(List<int> array)
            {
                int max = array[0];
                foreach (int num in array)
                {
                    if (num > max)
                        max = num;
                }
                return max;
            }

            private static void CountSort(List<int> array, int exp)
            {
                int n = array.Count;
                int[] output = new int[n];
                int[] count = new int[10];

                for (int i = 0; i < 10; i++)
                    count[i] = 0;

                for (int i = 0; i < n; i++)
                    count[(array[i] / exp) % 10]++;

                for (int i = 1; i < 10; i++)
                    count[i] += count[i - 1];

                for (int i = n - 1; i >= 0; i--)
                {
                    output[count[(array[i] / exp) % 10] - 1] = array[i];
                    count[(array[i] / exp) % 10]--;
                }

                for (int i = 0; i < n; i++)
                    array[i] = output[i];
            }

            public static List<int> ShellSort(List<int> array)
            {
                List<int> sortedArray = new List<int>(array);

                int n = sortedArray.Count;
                for (int gap = n / 2; gap > 0; gap /= 2)
                {
                    for (int i = gap; i < n; i += 1)
                    {
                        int temp = sortedArray[i];
                        int j;
                        for (j = i; j >= gap && sortedArray[j - gap] > temp; j -= gap)
                            sortedArray[j] = sortedArray[j - gap];
                        sortedArray[j] = temp;
                    }
                }

                return sortedArray;
            }

            public static List<int> QuickSort(List<int> array)
            {
                if (array.Count <= 1)
                    return new List<int>(array);

                int pivot = array[array.Count / 2];
                List<int> left = new List<int>();
                List<int> right = new List<int>();

                foreach (int num in array)
                {
                    if (num < pivot)
                        left.Add(num);
                    else if (num > pivot)
                        right.Add(num);
                }

                List<int> sortedArray = new List<int>();
                sortedArray.AddRange(QuickSort(left));
                sortedArray.Add(pivot);
                sortedArray.AddRange(QuickSort(right));

                return sortedArray;
            }

            public static List<int> TimSort(List<int> array)
            {
                List<int> sortedArray = new List<int>(array);

                const int run = 32;
                int n = sortedArray.Count;

                for (int i = 0; i < n; i += run)
                    InsertionSort(sortedArray, i, Math.Min((i + run - 1), (n - 1)));

                for (int size = run; size < n; size *= 2)
                {
                    for (int left = 0; left < n; left += 2 * size)
                    {
                        int mid = left + size - 1;
                        int right = Math.Min((left + 2 * size - 1), (n - 1));

                        if (mid < right)
                        {
                            Merge(sortedArray, left, mid, right);
                        }
                    }
                }

                return sortedArray;
            }

            private static void InsertionSort(List<int> array, int left, int right)
            {
                for (int i = left + 1; i <= right; i++)
                {
                    int temp = array[i];
                    int j = i - 1;
                    while (j >= left && array[j] > temp)
                    {
                        array[j + 1] = array[j];
                        j--;
                    }
                    array[j + 1] = temp;
                }
            }

            private static void Merge(List<int> array, int left, int mid, int right)
            {
                int n1 = mid - left + 1;
                int n2 = right - mid;

                int[] leftArray = new int[n1];
                int[] rightArray = new int[n2];

                for (int i = 0; i < n1; i++)
                    leftArray[i] = array[left + i];
                for (int j = 0; j < n2; j++)
                    rightArray[j] = array[mid + 1 + j];

                int k = left;
                int i1 = 0, i2 = 0;

                while (i1 < n1 && i2 < n2)
                {
                    if (leftArray[i1] <= rightArray[i2])
                    {
                        array[k] = leftArray[i1];
                        i1++;
                    }
                    else
                    {
                        array[k] = rightArray[i2];
                        i2++;
                    }
                    k++;
                }

                while (i1 < n1)
                {
                    array[k] = leftArray[i1];
                    i1++;
                    k++;
                }

                while (i2 < n2)
                {
                    array[k] = rightArray[i2];
                    i2++;
                    k++;
                }
            }
        }
    }
}
