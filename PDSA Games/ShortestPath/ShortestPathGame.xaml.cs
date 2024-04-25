using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace PDSA_Games
{
    public partial class ShortestPathGame : Window
    {
        private Dictionary<string, Dictionary<string, int>> distanceTable = new Dictionary<string, Dictionary<string, int>>();
        public string selectedCity;
        private Random random = new Random();
        string cityName;
        public string Username { get; set; }



        public ShortestPathGame()
        {
            InitializeComponent();
            
            
        }

        private void InitializeDistanceTable()
        {
            // Initialize the distance table with random distances between 5 and 50 km for each city
            distanceTable["CityA"] = new Dictionary<string, int>
    {
        { "CityB", random.Next(5, 51) },
        { "CityC", random.Next(5, 51) },
        { "CityD", random.Next(5, 51) },
        { "CityE", random.Next(5, 51) },
        { "CityF", random.Next(5, 51) },
        { "CityG", random.Next(5, 51) },
        { "CityH", random.Next(5, 51) }
    };
            distanceTable["CityB"] = new Dictionary<string, int>
    {
        { "CityA", random.Next(5, 51) },
        { "CityC", random.Next(5, 51) },
        { "CityD", random.Next(5, 51) },
        { "CityE", random.Next(5, 51) },
        { "CityF", random.Next(5, 51) },
        { "CityG", random.Next(5, 51) },
        { "CityH", random.Next(5, 51) }
    };
            distanceTable["CityC"] = new Dictionary<string, int>
    {
        { "CityA", random.Next(5, 51) },
        { "CityB", random.Next(5, 51) },
        { "CityD", random.Next(5, 51) },
        { "CityE", random.Next(5, 51) },
        { "CityF", random.Next(5, 51) },
        { "CityG", random.Next(5, 51) },
        { "CityH", random.Next(5, 51) }
    };
            distanceTable["CityD"] = new Dictionary<string, int>
    {
        { "CityA", random.Next(5, 51) },
        { "CityB", random.Next(5, 51) },
        { "CityC", random.Next(5, 51) },
        { "CityE", random.Next(5, 51) },
        { "CityF", random.Next(5, 51) },
        { "CityG", random.Next(5, 51) },
        { "CityH", random.Next(5, 51) }
    };
            distanceTable["CityE"] = new Dictionary<string, int>
    {
        { "CityA", random.Next(5, 51) },
        { "CityB", random.Next(5, 51) },
        { "CityC", random.Next(5, 51) },
        { "CityD", random.Next(5, 51) },
        { "CityF", random.Next(5, 51) },
        { "CityG", random.Next(5, 51) },
        { "CityH", random.Next(5, 51) }
    };
            distanceTable["CityF"] = new Dictionary<string, int>
    {
        { "CityA", random.Next(5, 51) },
        { "CityB", random.Next(5, 51) },
        { "CityC", random.Next(5, 51) },
        { "CityD", random.Next(5, 51) },
        { "CityE", random.Next(5, 51) },
        { "CityG", random.Next(5, 51) },
        { "CityH", random.Next(5, 51) }
    };
            distanceTable["CityG"] = new Dictionary<string, int>
    {
        { "CityA", random.Next(5, 51) },
        { "CityB", random.Next(5, 51) },
        { "CityC", random.Next(5, 51) },
        { "CityD", random.Next(5, 51) },
        { "CityE", random.Next(5, 51) },
        { "CityF", random.Next(5, 51) },
        { "CityH", random.Next(5, 51) }
    };
            distanceTable["CityH"] = new Dictionary<string, int>
    {
        { "CityA", random.Next(5, 51) },
        { "CityB", random.Next(5, 51) },
        { "CityC", random.Next(5, 51) },
        { "CityD", random.Next(5, 51) },
        { "CityE", random.Next(5, 51) },
        { "CityF", random.Next(5, 51) },
        { "CityG", random.Next(5, 51) }
    };
        }


        private void SelectRandomCity()
        {
            // Select a random city from the distance table
            int index = random.Next(distanceTable.Count);
            selectedCity = new List<string>(distanceTable.Keys)[index];
            txtSelectedCity.Text = selectedCity;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = (string)Application.Current.Resources["ConnectionString"];

            // Check player's answers
            cityName = txtCityName.Text.Trim();
            if (distanceTable.ContainsKey(cityName))
            {
                int playerDistance;
                if (int.TryParse(txtDistance.Text.Trim(), out playerDistance))
                {
                    if (playerDistance == GetShortestPath(selectedCity, cityName))
                    {
                        string answer = cityName +' '+ playerDistance.ToString();
                        txtFeedback.Text = "Correct!";
                        using (var connection = new SqlConnection(connectionString))
                        {
                            string insertQuery = "INSERT INTO ShortestPathGame (Username,Answer) VALUES (@Username,@Answer)";
                            connection.Execute(insertQuery, new { Username= Username, Answer =answer});
                        }

                    }
                    else
                    {
                        txtFeedback.Text = "Incorrect. The shortest distance to " + cityName + " from " + selectedCity + " is " + GetShortestPath(selectedCity, cityName) + " km.";
                    }
                }
                else
                {
                    txtFeedback.Text = "Invalid distance input. Please enter a number.";
                }
            }
            else
            {
                txtFeedback.Text = "Invalid city name. Please enter a valid city name.";
            }
        }

        private int GetShortestPath(string startCity, string endCity)
        {
            // Use Dijkstra's algorithm to find the shortest path distance
            Dictionary<string, int> distances = new Dictionary<string, int>();
            HashSet<string> visited = new HashSet<string>();

            foreach (var city in distanceTable.Keys)
            {
                distances[city] = int.MaxValue;
            }

            distances[startCity] = 0;

            while (visited.Count < distanceTable.Count)
            {
                string currentCity = null;
                int minDistance = int.MaxValue;

                foreach (var city in distanceTable.Keys)
                {
                    if (!visited.Contains(city) && distances[city] < minDistance)
                    {
                        currentCity = city;
                        minDistance = distances[city];
                    }
                }

                visited.Add(currentCity);

                foreach (var neighbor in distanceTable[currentCity])
                {
                    if (distances[currentCity] + neighbor.Value < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = distances[currentCity] + neighbor.Value;
                    }
                }
            }

            return distances[endCity];
        }

        private int BellmanFordShortestPath(string startCity, string endCity)
        {
            Dictionary<string, int> distances = new Dictionary<string, int>();

            foreach (var city in distanceTable.Keys)
            {
                distances[city] = int.MaxValue;
            }

            distances[startCity] = 0;

            for (int i = 0; i < distanceTable.Count - 1; i++)
            {
                foreach (var city in distanceTable.Keys)
                {
                    foreach (var neighbor in distanceTable[city])
                    {
                        if (distances[city] != int.MaxValue && distances[city] + neighbor.Value < distances[neighbor.Key])
                        {
                            distances[neighbor.Key] = distances[city] + neighbor.Value;
                        }
                    }
                }
            }

            return distances[endCity];
        }
        private void PopulateDistanceTable()
        {
            // Bind the distance table to the data grid
            dataGrid.ItemsSource = distanceTable.Select(kvp => new { City = kvp.Key, Distances = string.Join(", ", kvp.Value.Select(kv => $"{kv.Key}: {kv.Value} km")) }).ToList();
        }

        private void CheckAlgo_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = (string)Application.Current.Resources["ConnectionString"];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            BellmanFordShortestPath(selectedCity, cityName);
            stopwatch.Stop();
            TimeSpan BellmanTime = stopwatch.Elapsed;

            using (var connection = new SqlConnection(connectionString))
            {
                string BellmanQuery = "INSERT INTO ShortestPath (Method,TimeTaken) VALUES ('Bellman',@Time)";
                connection.Execute(BellmanQuery, new { Time = BellmanTime });
            }

            stopwatch.Restart();
            GetShortestPath(selectedCity, cityName);
            stopwatch.Stop();
            TimeSpan DijkstraTime = stopwatch.Elapsed;

            using (var connection = new SqlConnection(connectionString))
            {
                string DijikstraQuery = "INSERT INTO ShortestPath (Method,TimeTaken) VALUES ('Dijkstra',@Time)";
                connection.Execute(DijikstraQuery, new { Time = DijkstraTime });
            }

            txtAlgoTime.Content = "Bellman-Ford: " + BellmanTime + "\nDijkstra: " + DijkstraTime;
        }

        private void NewRound_Click(object sender, RoutedEventArgs e)
        {
            InitializeDistanceTable();
            PopulateDistanceTable();
            SelectRandomCity();
        }
    }
}
