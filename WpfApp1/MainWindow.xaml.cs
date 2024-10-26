using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using lib;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool RunFlag = true;
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            RunFlag = true;
            int count_population, count_city, lr, count_threads = 0;
            count_population = Convert.ToInt32(TextBox1.Text);
            count_city = Convert.ToInt32(TextBox2.Text);
            lr = Convert.ToInt32(TextBox3.Text);
            count_threads = Convert.ToInt32(TextBox4.Text);

            int[][] city_map_gen = GenMatrixCity(count_city);
            Point[] points = new Point[count_city];
            Random rand = new Random();
            for (int i = 0; i < count_city; i++)
            {
                double x = rand.NextDouble() * 400;
                double y = rand.NextDouble() * 400;
                points[i] = new Point(x, y);
            }
            Genom genom_rez = new Genom(count_city);
            var plotModel = CreatePlotModel(genom_rez.cityNumberConections, points);
            plot.Model = plotModel;
            await Task.Run(() =>
            {
                genom_rez = RunMulti(count_threads, genom_rez, count_population, city_map_gen, lr, plotModel, points);
            });
        }

        public Genom RunMulti(int count_thread, Genom bestgen, int count_population, int[][] WayLengMap, int lr, PlotModel plot, Point[] points)
        {
            bestgen.CalculateGenomWayLenght(WayLengMap);
            Task.Run(() =>
            {
                Debug.WriteLine("START DRAW");
                PlotDrow(plot, ref bestgen, points, WayLengMap);
            });
                Parallel.For(0, count_thread, i =>
            {
                Debug.WriteLine("START PODBOR");
                Population tm = new Population(count_population, WayLengMap, lr, i);
                tm.StartPopulationEvolution(ref bestgen, ref RunFlag);
            });
            Console.WriteLine("FIN");
            Console.WriteLine(bestgen.GenomScore);
            return bestgen;
        }

        private void PlotDrow(PlotModel plot, ref Genom genom, Point[] points, int[][] citymap)
        {
            int updates = 0;
            int score_best = genom.CalculateGenomWayLenght(citymap);
            Debug.WriteLine(RunFlag);
            while (RunFlag)
            {
                if (score_best < genom.CalculateGenomWayLenght(citymap))
                {
                    Debug.WriteLine(updates);
                    if (updates < 2)
                    {
                        AddEdges(plot, genom.cityNumberConections, points);
                        updates++;
                    }
                    else
                    {
                        UpdatePlot(plot, genom.cityNumberConections, points);
                        updates++;
                    }
                }
            }
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            RunFlag = false;
            Debug.WriteLine($"Stop button: {RunFlag}");
        }
        private PlotModel CreatePlotModel(int[] connectedVertices, Point[] positions)
        {
            var plotModel = new PlotModel { Title = "Graph" };
            var vertices = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = 10};
            var edges = new LineSeries { LineStyle = LineStyle.Solid, Color = OxyColors.Red, StrokeThickness = 5};
            foreach (int num_city in connectedVertices)
            {
                vertices.Points.Add(new ScatterPoint(positions[num_city].X, positions[num_city].Y));
            }

            edges.Points.Add(new DataPoint(positions[0].X, positions[0].Y));
            foreach (var num_city in connectedVertices)
            {
                edges.Points.Add(new DataPoint(positions[num_city].X, positions[num_city].Y));
            }
            plotModel.Series.Add(vertices);
            plotModel.Series.Add(edges);

            return plotModel;
        }

        private void AddEdges(PlotModel plot, int[] connectedVertices, Point[] positions)
        {
            int thick = 1;
            OxyColor[] colors = [OxyColors.Red, OxyColors.Green, OxyColors.Blue];
            foreach (var series in plot.Series.OfType<LineSeries>())
            {
                series.Color = colors[thick];
                series.StrokeThickness = 20 - 5 * thick;
                thick += 1;
            }
            var edges = new LineSeries { LineStyle = LineStyle.Solid, Color = OxyColors.Red, StrokeThickness = 5 };

            edges.Points.Add(new DataPoint(positions[0].X, positions[0].Y));
            foreach (var num_city in connectedVertices)
            {
                edges.Points.Add(new DataPoint(positions[num_city].X, positions[num_city].Y));
            }

            plot.Series.Add(edges);
            plot.InvalidatePlot(true);
        }

        private void UpdatePlot(PlotModel plot, int[] connectedVertices, Point[] positions)
        {
            plot.Series.RemoveAt(1);
            AddEdges(plot, connectedVertices, positions);
        }
        public static int[][] GenMatrixCity(int size)
        {
            Random random = new Random();
            int[][] matrix = new int[size][];
            for (int i = 0; i < size; i++)
            {
                matrix[i] = new int[size];
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    int distance = random.Next(1, 100);
                    matrix[i][j] = distance;
                    matrix[j][i] = distance;
                }
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    for (int k = j + 1; k < size; k++)
                    {
                        if (matrix[i][j] + matrix[j][k] < matrix[i][k])
                        {
                            matrix[i][k] = matrix[i][j] + matrix[j][k] + 1;
                            matrix[k][i] = matrix[i][k];
                        }
                    }
                }
            }

            return matrix;
        }

    }

}