using System;
using System.IO;
using System.Linq;
using Notrosoft_Accel.Infrastructure;
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

            plt.Grid(false);
            plt.Frame(false);
            plt.Ticks(false, false);

            plt.SaveFig(filePath);

            var fullPath = Path.GetFullPath(filePath);

            Console.WriteLine($"Saved pie chart to '{fullPath}'!");
        }
    }
}