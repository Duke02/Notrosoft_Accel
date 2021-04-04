﻿using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Interlayer interlayer = new();
        private static int colNum = 200; // Number of Columns present in the data structure.
        private static int rowNum = 50; // Number of Rows present in the data structure.
        public static List<List<string>> dataList = new(); // The data structure.

        public static DataTable
            dataTable = new(); // DataTable sits between the dataList of the backend and DataGrid of the GUI.

        public static List<StatisticType> statisticTypes = new();
        private readonly FileInput _fileImporter;

        private readonly GraphingWrapper _grapher;


        public MainWindow()
        {
            // The List<String> representing the default column List of size colNum.
            var colConstruct = new List<string>();

            for (var i = 0; i < colNum; i++) colConstruct.Add("");

            // Construct the rows as the empty column structure.
            for (var i = 0; i < rowNum; i++) dataList.Add(colConstruct);


            InitializeComponent();

            outputTextBlock.Text = "Testing 1, 2, 3";
            _grapher = new GraphingWrapper();

            _fileImporter = new FileInput();
        }

        // Constructs a dataTable object and binds it to the DataGrid's ItemsSource.
        private void dataGridTable()
        {
            // Set the dataTable to a new object.
            dataTable = new DataTable();

            // Column constructor
            for (var i = 0; i < colNum; i++)
            {
                // Instantiate a new DataColumn with a string type to be put into the DataTable.
                var newColumn = new DataColumn(i.ToString(), typeof(string));
                // Add the new column to the list of columns in the DataTable.
                dataTable.Columns.Add(newColumn);
            }

            // Creates a row object with the number of columns defined in the above loop.
            for (var i = 0; i < rowNum; i++)
            {
                // Instantiate a new row.
                var newRow = dataTable.NewRow();
                // Set the respective data of the DataTable to the dataList's. 
                for (var j = 0; j < colNum; j++) newRow[j] = dataList[i][j];

                dataTable.Rows.Add(newRow);
            }

            // Bind the DataGrid to the constructed dataTable.
            Data.ItemsSource = dataTable.DefaultView;
        }

        // Ensures the DataGrid is properly bound to the DataTable on window launch.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridTable();
        }

        // Handles the event of the user adding a column to the DataGrid.
        private void addColumnButton_Click(object sender, RoutedEventArgs e)
        {
            // Copies all data input to the dataTable back to the dataList for permanence.
            for (var i = 0; i < rowNum; i++)
            {
                var updatedRow = new List<string>();
                for (var j = 0; j < colNum; j++) updatedRow.Add(dataTable.Rows[i][j].ToString());

                dataList[i] = updatedRow;
            }

            // Inrease the column number.
            colNum++;

            // Add an additional empty column to each row in the list.
            for (var i = 0; i < rowNum; i++) dataList[i].Add("");

            // Redo the dataTable to DataGrid binding.
            dataGridTable();
        }

        // Defines the Row headers
        private void Data_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var header = ' ' + e.Row.GetIndex().ToString();
            e.Row.Header = header;
        }

        // Adding rows handler.
        private void addRowButton_Click(object sender, RoutedEventArgs e)
        {
            // Copies all data input to the dataTable back to the dataList for permanence.
            for (var i = 0; i < rowNum; i++)
            {
                var updatedRow = new List<string>();
                for (var j = 0; j < colNum; j++) updatedRow.Add(dataTable.Rows[i][j].ToString());

                dataList[i] = updatedRow;
            }

            // Inrease the column number.
            rowNum++;
            var emptyCol = new List<string>();
            // Add an additional empty column to each row in the list.
            for (var i = 0; i < colNum; i++) emptyCol.Add("");

            dataList.Add(emptyCol);
            // Redo the dataTable to DataGrid binding.
            dataGridTable();
        }

        private void GraphButton_OnClick(object sender, RoutedEventArgs e)
        {
            var data = new FrequencyData<string>(new Dictionary<string, int>
            {
                {"Cats", 30},
                {"Dogs", 45},
                {"Parrots", 12}
            });

            var savePieChartToDialog = new SaveFileDialog
            {
                Filter = "JPeg Image|*.jpg",
                Title = "Save graph to..."
            };

            savePieChartToDialog.ShowDialog(this);

            if (!string.IsNullOrWhiteSpace(savePieChartToDialog.FileName))
                _grapher.PlotPieChart(data, savePieChartToDialog.FileName);
            else
                MessageBox.Show("Cannot save graph to an empty file location!");
        }

        private void Data_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
        }

        private void ImportFile_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV|*.csv",
                Title = "Import CSV..."
            };

            openFileDialog.ShowDialog(this);

            if (string.IsNullOrWhiteSpace(openFileDialog.FileName) || !File.Exists(openFileDialog.FileName))
            {
                MessageBox.Show("Cannot find file!");
            }
            else
            {
                var newData = _fileImporter.ReadFile(openFileDialog.FileName);
                // TODO: Overwrite the data in the table with this new stuff.
            }
        }

        private void doStatsButton_Click(object sender, RoutedEventArgs e)
        {
            var try1 = new List<List<string>>();
            var test = new List<string> {"1", "3", "5", "6", "6", "7.2", "0.225", "2.4"};
            try1.Add(test);
            try1.Add(test);
            outputTextBlock.Text = interlayer.doStatistics(try1, statisticTypes.ToArray(), null);
        }

        // ------------------------ CHECKBOX HANDLERS ------------------------
        // -------------------------------------------------------------------

        private void MeanButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.Mean);
        }

        private void MeanButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.Mean);
        }

        private void MedianButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.Median);
        }

        private void MedianButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.Median);
        }

        private void ModeButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.Mode);
        }

        private void ModeButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.Mode);
        }

        private void StandardDeviationButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.StandardDeviation);
        }

        private void StandardDeviationButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.StandardDeviation);
        }

        private void VarianceButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.Variance);
        }

        private void VarianceButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.Variance);
        }

        private void CoefficientOfVarianceButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.CoefficientOfVariance);
        }

        private void CoefficientOfVarianceButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.CoefficientOfVariance);
        }

        private void PercentileButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.Percentile);
        }

        private void PercentileButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.Percentile);
        }

        private void ProbabilityDistributionButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.ProbabilityDistribution);
        }

        private void ProbabilityDistributionButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.ProbabilityDistribution);
        }

        private void BinomialDistributionButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.BinomialDistribution);
        }

        private void BinomialDistributionButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.BinomialDistribution);
        }

        private void LeastSquaresLineButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.LeastSquaresLine);
        }

        private void LeastSquaresLineButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.LeastSquaresLine);
        }

        private void ChiSquareButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.ChiSquare);
        }

        private void ChiSquareButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.ChiSquare);
        }

        private void CorrelationCoefficientButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.CorrelationCoefficient);
        }

        private void CorrelationCoefficientButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.CorrelationCoefficient);
        }

        private void SignTestButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.SignTest);
        }

        private void SignTestButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.SignTest);
        }

        private void RankSumTestButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.RankSumTest);
        }

        private void RankSumTestButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.RankSumTest);
        }

        private void SpearmanRankCorrelationCoefficientButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Add(StatisticType.SpearmanRankCorrelationCoefficient);
        }

        private void SpearmanRankCorrelationCoefficientButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypes.Remove(StatisticType.SpearmanRankCorrelationCoefficient);
        }
    }
}