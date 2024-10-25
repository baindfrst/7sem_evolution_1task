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
        private bool RunFlag = true;
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            int count_population, count_city, lr, count_threads = 0;
            count_population = Convert.ToInt32(TextBox1.Text);
            count_city = Convert.ToInt32(TextBox2.Text);
            lr = Convert.ToInt32(TextBox3.Text);
            count_threads = Convert.ToInt32(TextBox4.Text);

            int[][] city_map_gen = GenMatrixCity(count_city);

            Population population_cr = new Population(count_population, city_map_gen, lr);
            Genom best_gen = population_cr.bestGen;
            int[] rez_gen_conection = best_gen.cityNumberConections;
            int best_score = best_gen.CalculateGenomWayLenght(city_map_gen);
            Point[] points = new Point[count_city];
            Random rand = new Random();
            for (int i = 0; i < count_city; i++)
            {
                double x = rand.NextDouble() * 400;
                double y = rand.NextDouble() * 400;
                points[i] = new Point(x, y);
            }
            var plotModel = CreatePlotModel(rez_gen_conection, points);
            plot.Model = plotModel;

            int count_update = 0;
            while (RunFlag)
            {
                if (best_score < best_gen.CalculateGenomWayLenght(city_map_gen))
                {
                    if (count_update < 2)
                    {
                        AddEdges(plotModel, best_gen.cityNumberConections, points);
                        count_update++;
                    }
                    else
                    {
                        UpdatePlot(plotModel, best_gen.cityNumberConections, points);
                        count_update++;
                    }
                }
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            RunFlag = false;
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