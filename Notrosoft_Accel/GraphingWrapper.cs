using System;
using System.IO;
using System.Linq;
using Notrosoft_Accel.Infrastructure;
using System.Collections.Generic;
using ScottPlot;

namespace Notrosoft_Accel
{
    public class GraphingWrapper
    {
        public void PlotPieChart<T>(FrequencyData<T> data, string filePath, int width = 600, int height = 400)
        {
            // Create the parent directory if it doesn't already exist.
            var parentDirectoryPath = Directory.GetParent(filePath)?.FullName;
            if (!string.IsNullOrWhiteSpace(parentDirectoryPath) && !Directory.Exists(parentDirectoryPath))
                Directory.CreateDirectory(parentDirectoryPath);

            var plt = new Plot(width, height);

            var values = data.Values.Select(i => (double) i).ToArray();

            var labels = data.Keys.Select(k => k.ToString()).ToArray();

            plt.PlotPie(values, labels, showValues: true, showPercentages: true);

            var fullPath = Path.GetFullPath(filePath);

            plt.Grid(false);
            plt.Frame(false);
            plt.Ticks(false, false);

            var title = Path.GetFileNameWithoutExtension(filePath);
            plt.Title(title);

            plt.SaveFig(filePath);

            Console.WriteLine($"Saved pie chart to '{fullPath}'!");
        }

        public void PlotBarChart(FrequencyData<string> data, string filePath, int width = 600, int height = 400,
            char direction = 'v')
        {
            // Create the parent directory if it doesn't already exist.
            var parentDirectoryPath = Directory.GetParent(filePath)?.FullName;
            if (!string.IsNullOrWhiteSpace(parentDirectoryPath) && !Directory.Exists(parentDirectoryPath))
                Directory.CreateDirectory(parentDirectoryPath);

            var plt = new Plot(width, height);

            var values = data.Values.Select(i => (double) i).ToArray();

            var labels = data.Keys.Select(k => k.ToString()).ToArray();
            var barCount = DataGen.Consecutive(values.Length);
            if (direction == 'v')
            {
                plt.PlotBar(barCount, values, null, horizontal: false);
                plt.Grid(enableHorizontal: true);
                plt.XTicks(barCount, labels);
            }
            else
            {
                plt.PlotBar(barCount, values, null, horizontal: true);
                plt.Grid(enableHorizontal: false);
                plt.YTicks(barCount, labels);
            }

            var title = Path.GetFileNameWithoutExtension(filePath);
            plt.Title(title);
            plt.SaveFig(filePath);

            var fullPath = Path.GetFullPath(filePath);

            Console.WriteLine($"Saved pie chart to '{fullPath}'!");
        }

        public void PlotXYGraph(List<double> data, string filePath, int width = 600, int height = 400)
        {
            // Create the parent directory if it doesn't already exist.
            var parentDirectoryPath = Directory.GetParent(filePath)?.FullName;
            if (!string.IsNullOrWhiteSpace(parentDirectoryPath) && !Directory.Exists(parentDirectoryPath))
                Directory.CreateDirectory(parentDirectoryPath);

            var plt = new Plot(width, height);

            List<double> xs = new();
            List<double> ys = new();

            for (int i = 0; i < data.Count; i += 2)
            {
                xs.Add(data[i]);
                ys.Add(data[i + 1]);
            }


            var fullPath = Path.GetFullPath(filePath);

            plt.PlotScatter(xs.ToArray(), ys.ToArray());
            plt.YLabel("Y");
            plt.XLabel("X");

            var title = Path.GetFileNameWithoutExtension(filePath);
            plt.Title(title);

            plt.SaveFig(filePath);

            Console.WriteLine($"Saved pie chart to '{fullPath}'!");
        }

        public void PlotNormalGraph(List<double> data, string filePath, int width = 600, int height = 400)
        {
        }
    }
}