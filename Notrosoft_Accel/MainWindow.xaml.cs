using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
        private static int colNum = 50; // Number of Columns present in the data structure.
        private static int rowNum = 100; // Number of Rows present in the data structure.
        public static List<List<string>> dataList = new(); // The data structure.

        public static DataTable
            dataTable = new(); // DataTable sits between the dataList of the backend and DataGrid of the GUI.

        public static List<StatisticType> ordinalStats = new();
        public static List<StatisticType> intervalStats = new();
        public static List<StatisticType> frequencyStats = new();
        private readonly FileInput _fileImporter;

        private readonly GraphingWrapper _grapher;

        private static Dictionary<string, IEnumerable<double>> interDataDef = new();

        public MainWindow()
        {
            for (var i = 0; i < rowNum; i++)
            {
                // The List<String> representing the default column List of size colNum.
                var colConstruct = new List<string>();

                for (var j = 0; j < colNum; j++) colConstruct.Add("");

                // Construct the rows as the empty column structure.
                dataList.Add(colConstruct);
            }

            InitializeComponent();

            outputTextBlock.Text = "Testing 1, 2, 3";
            _grapher = new GraphingWrapper();

            _fileImporter = new FileInput();
        }

        // Constructs a dataTable object and binds it to the DataGrid's ItemsSource.
        private void dataGridTable(int oldC, int oldR)
        {
            // Set the dataTable to a new object.
            if (oldR == rowNum)
            {
                dataTable = new DataTable();
                oldR = 0;
                oldC = 0;
            }

            // Column constructor
            for (var i = oldC; i < colNum; i++)
            {
                // Instantiate a new DataColumn with a string type to be put into the DataTable.
                var newColumn = new DataColumn(i.ToString(), typeof(string));
                // Add the new column to the list of columns in the DataTable.
                dataTable.Columns.Add(newColumn);
            }
            
            // Creates a row object with the number of columns defined in the above loop.
            for (var row = oldR; row < rowNum; row++)
            {
                // Instantiate a new row.
                var newRow = dataTable.NewRow();
                // Set the respective data of the DataTable to the dataList's. 
                for (var col = 0; col < colNum; col++) newRow[col] = dataList[row][col];

                dataTable.Rows.Add(newRow);
            }

            // Bind the DataGrid to the constructed dataTable.
            Data.ItemsSource = dataTable.DefaultView;
        }

        // Ensures the DataGrid is properly bound to the DataTable on window launch.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridTable(0,0);
        }

        // Handles the event of the user adding a column to the DataGrid.
        private void addColumnButton_Click(object sender, RoutedEventArgs e)
        {
            // Inrease the column number.
            if (colNum >= 200)
            {
                colNum = 200;
                MessageBox.Show("Only 200 columns allowed max");
            }
            else
            {
                colNum++;

                // Add an additional empty column to each row in the list.
                for (var i = 0; i < rowNum; i++) dataList[i].Add("");

                // Redo the dataTable to DataGrid binding.
                dataGridTable(colNum - 1, rowNum);
            }
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
            // Inrease the column number.
            rowNum++;
            var emptyCol = new List<string>();
            // Add an additional empty column to each row in the list.
            for (var i = 0; i < colNum; i++) emptyCol.Add("");

            dataList.Add(emptyCol);
            // Redo the dataTable to DataGrid binding.
            dataGridTable(colNum, rowNum - 1);
        }

        private void GraphButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (pieChartButton.Visibility == Visibility.Visible)
            {
                pieChartButton.Visibility = Visibility.Collapsed;
                horBarButton.Visibility = Visibility.Collapsed;
                verBarButton.Visibility = Visibility.Collapsed;
                xyButton.Visibility = Visibility.Collapsed;
                normalDistButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                pieChartButton.Visibility = Visibility.Visible;
                horBarButton.Visibility = Visibility.Visible;
                verBarButton.Visibility = Visibility.Visible;
                xyButton.Visibility = Visibility.Visible;
                normalDistButton.Visibility = Visibility.Visible;
            }
        }

        private void Data_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
        }

        private void DataExport_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV|*.csv",
                Title = "Save Data..."
            };

            var gotValidPath = saveFileDialog.ShowDialog(this) ?? false;

            if (!gotValidPath) return;

            if (string.IsNullOrWhiteSpace(saveFileDialog.FileName))
            {
                MessageBox.Show("Invalid save path!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var data = new List<string>();

                for (var col = 0; col < colNum; col++)
                    data.Add(string.Join(",", dataTable.Rows[col].ItemArray.Select(i => i.ToString())));

                try
                {
                    File.WriteAllLines(saveFileDialog.FileName, data);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"There was an error saving the spreadsheet!\n{ex.Message}", "Error saving data!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ImportFile_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV|*.csv",
                Title = "Import CSV..."
            };

            var wasOpened = openFileDialog.ShowDialog(this) ?? false;

            if (!wasOpened) return;

            if (string.IsNullOrWhiteSpace(openFileDialog.FileName) || !File.Exists(openFileDialog.FileName))
            {
                MessageBox.Show("Cannot find file!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var newData = new List<List<string>>();

                try
                {
                    newData = _fileImporter.ReadFile(openFileDialog.FileName);
                }
                catch (IOException ioE)
                {
                    MessageBox.Show($"There was an error opening the file!\n{ioE.Message}", "Error opening file!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newColsNeeded = newData.Max(r => r.Count);
                var newRowsNeeded = newData.Count;

                var rowsNeeded = Math.Max(newRowsNeeded, rowNum);
                var colsNeeded = Math.Max(newColsNeeded, colNum);

                // Pad the newData to fit the previous size of the data table.
                for (var row = newRowsNeeded; row < rowsNeeded; row++) newData.Add(new List<string>());

                for (var row = 0; row < rowsNeeded; row++)
                {
                    var newColsNeededForRow = colsNeeded - newData[row].Count;
                    for (var col = 0; col < newColsNeededForRow; col++) newData[row].Add(string.Empty);
                }

                colNum = colsNeeded;
                rowNum = rowsNeeded;

                dataList = newData;

                // Redo the dataTable to DataGrid binding.
                dataGridTable(colNum, rowNum);
            }
        }

        private void doStatsButton_Click(object sender, RoutedEventArgs e)
        {
            
            var sel = Data.SelectedCells;
            var try1 = new List<List<string>>();
            int lastC = int.MaxValue, thisC;
            var i = -1;
            foreach (var cellInfo in sel)
                // Ensures cell information is valid. If not then don't try and do anything with it
                if (cellInfo.IsValid)
                {
                    thisC = cellInfo.Column.DisplayIndex;
                    if (thisC < lastC)
                    {
                        i++;
                        try1.Add(new List<string>());
                    }

                    lastC = thisC;

                    // Get's the current cell's information (specifically for Column info)
                    var cont = cellInfo.Column.GetCellContent(cellInfo.Item);
                    // Get's the current row's information
                    var row = (DataRowView) cont.DataContext;
                    // Get the current row's data as an item array
                    var obj = row.Row.ItemArray;
                    // Add the item of the current row at the current column's index and add it to the outbound list.
                    try1[i].Add(obj[thisC].ToString());
                }
            outputTextBlock.Text += "Statistics performed at " + System.DateTime.Today + "\n";
            // Ordinal Data
            if (StatTypeBox.SelectedIndex == 0)
            {
                outputTextBlock.Text += interlayer.doStatistics(try1, ordinalStats.ToArray(), null, null) + '\n';
            }
            // Frequency Data
            else if (StatTypeBox.SelectedIndex == 1) MessageBox.Show("Frequency Statistics not implemented yet");

            // Interval Data
            else if (StatTypeBox.SelectedIndex == 2)
            {
                outputTextBlock.Text += interlayer.doStatistics(try1, intervalStats.ToArray(), interDataDef, null) + '\n';
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Data.MaxHeight = e.NewSize.Height - 110;
        }

        // WIP to maybe make the row and column additions faster
        private void Data_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string qq = e.Column.GetCellContent(e.Row.Item).ToString();
            var theOut = "";
            if (qq.Length > 33)
            {
                theOut = qq.Substring(33);
            }
            int rN = e.Row.GetIndex();
            //dataTable.Rows[rN][e.Column.DisplayIndex] = theOut;
            dataList[rN][e.Column.DisplayIndex] = theOut;
        }

        // ------------------------ CHECKBOX HANDLERS ------------------------
        // -------------------------------------------------------------------

        private void MeanButton_Checked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Add(StatisticType.Mean);
        }

        private void MeanButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.Mean);
        }

        private void MedianButton_Checked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Add(StatisticType.Median);
        }

        private void MedianButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.Median);
        }

        private void ModeButton_Checked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Add(StatisticType.Mode);
        }

        private void ModeButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.Mode);
        }

        private void StandardDeviationButton_Checked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Add(StatisticType.StandardDeviation);
        }

        private void StandardDeviationButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.StandardDeviation);
        }

        private void VarianceButton_Checked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Add(StatisticType.Variance);
        }

        private void VarianceButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.Variance);
        }

        private void CoefficientOfVarianceButton_Checked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Add(StatisticType.CoefficientOfVariance);
        }

        private void CoefficientOfVarianceButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.CoefficientOfVariance);
        }

        private void PercentileButton_Checked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Add(StatisticType.Percentile);
        }

        private void PercentileButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.Percentile);
        }

        private void ProbabilityDistributionButton_Checked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Add(StatisticType.ProbabilityDistribution);
        }

        private void ProbabilityDistributionButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.ProbabilityDistribution);
        }

        private void BinomialDistributionButton_Checked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Add(StatisticType.BinomialDistribution);
        }

        private void BinomialDistributionButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.BinomialDistribution);
        }

        private void LeastSquaresLineButton_Checked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Add(StatisticType.LeastSquaresLine);
        }

        private void LeastSquaresLineButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.LeastSquaresLine);
        }

        private void ChiSquareButton_Checked(object sender, RoutedEventArgs e)
        {
            intervalStats.Add(StatisticType.ChiSquare);
        }

        private void ChiSquareButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.ChiSquare);
        }

        private void CorrelationCoefficientButton_Checked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Add(StatisticType.CorrelationCoefficient);
        }

        private void CorrelationCoefficientButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.CorrelationCoefficient);
        }

        private void SignTestButton_Checked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Add(StatisticType.SignTest);
        }

        private void SignTestButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.SignTest);
        }

        private void RankSumTestButton_Checked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Add(StatisticType.RankSumTest);
        }

        private void RankSumTestButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.RankSumTest);
        }

        private void SpearmanRankCorrelationCoefficientButton_Checked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Add(StatisticType.SpearmanRankCorrelationCoefficient);
        }

        private void SpearmanRankCorrelationCoefficientButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ordinalStats.Remove(StatisticType.SpearmanRankCorrelationCoefficient);
        }

        // ----------------------- DATA TYPES HANDLER ------------------------
        // -------------------------------------------------------------------

        private void setInterval_Click(object sender, RoutedEventArgs e)
        {
            var sel = Data.SelectedCells;
            var try1 = new List<string>();
            int lastC = int.MaxValue, thisC;
            int count = 0;
            foreach (var cellInfo in sel)
            {
                // Ensures cell information is valid. If not then don't try and do anything with it
                if (cellInfo.IsValid)
                {
                    count++;
                    thisC = cellInfo.Column.DisplayIndex;

                    lastC = thisC;

                    // Get's the current cell's information (specifically for Column info)
                    var cont = cellInfo.Column.GetCellContent(cellInfo.Item);
                    // Get's the current row's information
                    var row = (DataRowView)cont.DataContext;
                    // Get the current row's data as an item array
                    var obj = row.Row.ItemArray;
                    // Add the item of the current row at the current column's index and add it to the outbound list.
                    try1.Add(obj[thisC].ToString());
                }
            }
            if ((count % 3) != 0) MessageBox.Show("Interval input formatted incorrectly;\nplease have the interval name, lower numerical limit, then upper numerical limit for every interval",
                "Format Error", MessageBoxButton.OK);
            else
            {
                interDataDef.Clear();
                for (int i = 0; i < (count/3); i++)
                {
                    double a = double.Parse(try1[(3 * i) + 1]);
                    double b = double.Parse(try1[(3 * i) + 2]);
                    List<double> tAB = new List<double>{ a, b };
                    interDataDef.Add(try1[(3 * i)], tAB);
                }

                if (doStatsButton.IsEnabled == false) doStatsButton.IsEnabled = true;
            }
            
        }

        // Interval
        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            setInterval.IsEnabled = true;
            setInterval.Visibility = Visibility.Visible;
            doStatsButton.IsEnabled = false;
        }
        // Frequency
        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            setInterval.IsEnabled = false;
            setInterval.Visibility = Visibility.Collapsed;
        }
        // Ordinal (since this is the first one active on launch; it has to check if things are null or not)
        private void ComboBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            if (doStatsButton != null) doStatsButton.IsEnabled = true;
            if (setInterval != null)
            {
                setInterval.IsEnabled = false;
                setInterval.Visibility = Visibility.Collapsed;
            }
        }

        private void pieChartButton_Click(object sender, RoutedEventArgs e)
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

        private void horBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void verBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void xyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void normalDistButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}