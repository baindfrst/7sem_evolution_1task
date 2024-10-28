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

            Genom genom_rez = new Genom(count_city);
            PlotMC plot_cl = new PlotMC(count_city, genom_rez.cityNumberConections);
            plot.Model = plot_cl.plot;
            //RunMulti(count_threads, genom_rez, count_population, city_map_gen, lr, plot_cl);
        }

        public Genom RunMulti(int count_thread, Genom bestgen, int count_population, int[][] WayLengMap, int lr, PlotMC plot)
        {
            bestgen.CalculateGenomWayLenght(WayLengMap);
            Parallel.For(0, count_thread, i =>
            {
                Debug.WriteLine("START PODBOR");
                Population tm = new Population(count_population, WayLengMap, lr, i);
                tm.StartPopulationEvolution(ref bestgen, ref RunFlag, plot);
            });
            Console.WriteLine("FIN");
            Console.WriteLine(bestgen.GenomScore);
            return bestgen;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            RunFlag = false;
            Debug.WriteLine($"Stop button: {RunFlag}");
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