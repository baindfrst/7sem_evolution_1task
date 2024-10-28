using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib;
using OxyPlot;
using OxyPlot.Series;

namespace lib;

public class PlotMC
{
    public PlotModel plot;
    public double[][] cords;
    public int updates;
    public PlotMC(int count_c, int[] city_conect)
    {
        updates = 0;
        cords = new double[count_c][];
        Random rand = new Random();
        for (int i = 0; i < count_c; i++)
        {
            double x = rand.NextDouble() * 400;
            double y = rand.NextDouble() * 400;
            cords[i] = [x, y];
        }
        plot = new PlotModel { Title = "Graph" };
        var vertices = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = 10 };
        var edges = new LineSeries { LineStyle = LineStyle.Solid, Color = OxyColors.Red, StrokeThickness = 5 };
        foreach (int num_city in city_conect)
        {
            vertices.Points.Add(new ScatterPoint(cords[num_city][0], cords[num_city][1]));
        }

        edges.Points.Add(new DataPoint(cords[0][0], cords[0][1]));
        foreach (var num_city in city_conect)
        {
            edges.Points.Add(new DataPoint(cords[num_city][0], cords[num_city][1]));
        }
        plot.Series.Add(vertices);
        plot.Series.Add(edges);
    }

    public void AddEdges(PlotModel plot, int[] connectedVertices, double[][] positions)
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

        edges.Points.Add(new DataPoint(positions[0][0], positions[0][1]));
        foreach (var num_city in connectedVertices)
        {
            edges.Points.Add(new DataPoint(positions[num_city][0], positions[num_city][1]));
        }

        plot.Series.Add(edges);
        plot.InvalidatePlot(true);
    }
    public void UpdatePlot(PlotModel plot, int[] connectedVertices, double[][] positions)
    {
        plot.Series.RemoveAt(1);
        AddEdges(plot, connectedVertices, positions);
    }

    public void PlotNewDrow(int[] conections)
    {
        if (updates < 2)
        {
            AddEdges(plot, conections, cords);
            updates++;
        }
        else
        {
            UpdatePlot(plot, conections, cords);
            updates++;
        }
    }
}
