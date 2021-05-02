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

        public Dictionary<string, double> Parameters = new();

        public static DataTable
            dataTable = new(); // DataTable sits between the dataList of the backend and DataGrid of the GUI.

        public static List<StatisticType> statisticTypesToPerform = new();
        private readonly FileInput _fileImporter;

        private readonly GraphingWrapper _grapher;

        private static Dictionary<string, IEnumerable<double>> interDataDef = null;

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

        #region DataGrid manipulation

        // Constructs a dataTable object and binds it to the DataGrid's ItemsSource.
        private void ConstructDataGridTable(int oldC, int oldR)
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
            ConstructDataGridTable(0, 0);
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
                ConstructDataGridTable(colNum - 1, rowNum);
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
            ConstructDataGridTable(colNum, rowNum - 1);
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

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Data.MaxHeight = e.NewSize.Height - 110;
        }

        #endregion


        private void Data_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
        }

        #region Data from file

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
                List<List<string>> newData;

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
                ConstructDataGridTable(colNum, rowNum);
            }
        }

        #endregion

        #region Statistics

        private void doStatsButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCells = Data.SelectedCells;
            var statsInput = new List<List<string>>();
            var lastColumn = int.MaxValue;
            int currentColumn;
            var i = -1;
            foreach (var cellInfo in selectedCells)
            {
                // Ensures cell information is valid. If not then don't try and do anything with it
                if (cellInfo.IsValid)
                {
                    currentColumn = cellInfo.Column.DisplayIndex;
                    if (currentColumn < lastColumn)
                    {
                        i++;
                        statsInput.Add(new List<string>());
                    }

                    lastColumn = currentColumn;

                    // Get's the current cell's information (specifically for Column info)
                    var cont = cellInfo.Column.GetCellContent(cellInfo.Item);
                    // Get's the current row's information
                    var row = (DataRowView) cont.DataContext;
                    // Get the current row's data as an item array
                    var obj = row.Row.ItemArray;
                    // Add the item of the current row at the current column's index and add it to the outbound list.
                    statsInput[i].Add(obj[currentColumn].ToString());
                }
            }

            // Currently statsInput is oriented as rows x colums, when stats expects it to be columns x rows.
            if (statsInput.Count > 1)
            {
                statsInput = Utilities.Transpose(statsInput).Select(l => l.ToList()).ToList();
            }


            outputTextBlock.Text += "Statistics performed at " + DateTime.Now + "\n";

            try
            {
                var dataType = StatTypeBox.SelectedIndex switch
                {
                    0 => DataType.Ordinal,
                    1 => DataType.Frequency,
                    2 => DataType.Interval,
                    _ => throw new InvalidOperationException("Somehow got an index we didn't expect!")
                };

                var intervalDefinitions = interDataDef != null ? new IntervalDefinitions(interDataDef) : null;

                outputTextBlock.Text += interlayer.doStatistics(statsInput, statisticTypesToPerform.ToArray(), dataType,
                    intervalDefinitions, Parameters.Values.ToArray()) + '\n';
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Encountered a problem...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // ------------------------ CHECKBOX HANDLERS ------------------------
        // -------------------------------------------------------------------

        private void MeanButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.Mean);
        }

        private void MeanButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.Mean);
        }

        private void MedianButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.Median);
        }

        private void MedianButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.Median);
        }

        private void ModeButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.Mode);
        }

        private void ModeButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.Mode);
        }

        private void StandardDeviationButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.StandardDeviation);
        }

        private void StandardDeviationButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.StandardDeviation);
        }

        private void VarianceButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.Variance);
        }

        private void VarianceButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.Variance);
        }

        private void CoefficientOfVarianceButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.CoefficientOfVariance);
        }

        private void CoefficientOfVarianceButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.CoefficientOfVariance);
        }

        private void PercentileButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.Percentile);
        }

        private void PercentileButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.Percentile);
        }

        private void ProbabilityDistributionButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.ProbabilityDistribution);
        }

        private void ProbabilityDistributionButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.ProbabilityDistribution);
        }

        private void BinomialDistributionButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.BinomialDistribution);
        }

        private void BinomialDistributionButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.BinomialDistribution);
        }

        private void LeastSquaresLineButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.LeastSquaresLine);
        }

        private void LeastSquaresLineButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.LeastSquaresLine);
        }

        private void ChiSquareButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.ChiSquare);
        }

        private void ChiSquareButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.ChiSquare);
        }

        private void CorrelationCoefficientButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.CorrelationCoefficient);
        }

        private void CorrelationCoefficientButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.CorrelationCoefficient);
        }

        private void SignTestButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.SignTest);
        }

        private void SignTestButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.SignTest);
        }

        private void RankSumTestButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.RankSumTest);
        }

        private void RankSumTestButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.RankSumTest);
        }

        private void SpearmanRankCorrelationCoefficientButton_Checked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Add(StatisticType.SpearmanRankCorrelationCoefficient);
        }

        private void SpearmanRankCorrelationCoefficientButton_Unchecked(object sender, RoutedEventArgs e)
        {
            statisticTypesToPerform.Remove(StatisticType.SpearmanRankCorrelationCoefficient);
        }

        #endregion

        #region DataTypes

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
                    var content = cellInfo.Column.GetCellContent(cellInfo.Item);
                    // Get's the current row's information
                    var row = (DataRowView) content.DataContext;
                    // Get the current row's data as an item array
                    var obj = row.Row.ItemArray.Select(i => i?.ToString() ?? string.Empty).ToArray();
                    // Add the item of the current row at the current column's index and add it to the outbound list.
                    try1.Add(obj[thisC].ToString());
                }
            }

            if ((count % 3) != 0)
                MessageBox.Show(
                    "Interval input formatted incorrectly;\nplease have the interval name, lower numerical limit, then upper numerical limit for every interval",
                    "Format Error", MessageBoxButton.OK);
            else
            {
                if (interDataDef == null)
                {
                    interDataDef = new Dictionary<string, IEnumerable<double>>();
                }

                try
                {
                    interDataDef.Clear();
                    for (var i = 0; i < (count / 3); i++)
                    {
                        var a = double.Parse(try1[(3 * i) + 1]);
                        var b = double.Parse(try1[(3 * i) + 2]);
                        var tAB = new List<double> {a, b};
                        interDataDef.Add(try1[(3 * i)], tAB);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Encountered error parsing interval definitions! {ex.Message}", "Encountered a problem...", MessageBoxButton.OK, MessageBoxImage.Error);
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

        #endregion

        #region Graphing

        // ----------------------- GRAPHING HANDLER --------------------------
        // -------------------------------------------------------------------
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

        private void pieChartButton_Click(object sender, RoutedEventArgs e)
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
                    var row = (DataRowView) cont.DataContext;
                    // Get the current row's data as an item array
                    var obj = row.Row.ItemArray;
                    // Add the item of the current row at the current column's index and add it to the outbound list.
                    try1.Add(obj[thisC].ToString());
                }
            }

            var dic = new Dictionary<string, int>();
            for (int i = 0; i < try1.Count(); i += 2)
            {
                dic.Add(try1[i], int.Parse(try1[i + 1]));
            }

            var data = new FrequencyData<string>(dic);

            var saveChartToDialog = new SaveFileDialog
            {
                Filter = "JPeg Image|*.jpg",
                Title = "Save graph to..."
            };

            saveChartToDialog.ShowDialog(this);

            if (!string.IsNullOrWhiteSpace(saveChartToDialog.FileName))
            {
                _grapher.PlotPieChart(data, saveChartToDialog.FileName);
                ImageViewWindow PieChart = new(saveChartToDialog.FileName);
                PieChart.Show();
            }
            else
            {
                MessageBox.Show("Graph not saved.");
            }
        }

        private void horBarButton_Click(object sender, RoutedEventArgs e)
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
                    var row = (DataRowView) cont.DataContext;
                    // Get the current row's data as an item array
                    var obj = row.Row.ItemArray;
                    // Add the item of the current row at the current column's index and add it to the outbound list.
                    try1.Add(obj[thisC].ToString());
                }
            }

            var dic = new Dictionary<string, int>();
            for (int i = 0; i < try1.Count(); i += 2)
            {
                dic.Add(try1[i], int.Parse(try1[i + 1]));
            }

            var data = new FrequencyData<string>(dic);

            var saveChartToDialog = new SaveFileDialog
            {
                Filter = "JPeg Image|*.jpg",
                Title = "Save graph to..."
            };

            saveChartToDialog.ShowDialog(this);

            if (!string.IsNullOrWhiteSpace(saveChartToDialog.FileName))
            {
                _grapher.PlotBarChart(data, saveChartToDialog.FileName, 600, 400, 'h');
                ImageViewWindow BarChart = new(saveChartToDialog.FileName);
                BarChart.Show();
            }
            else
            {
                MessageBox.Show("Graph not saved.");
            }
        }

        private void verBarButton_Click(object sender, RoutedEventArgs e)
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
                    var row = (DataRowView) cont.DataContext;
                    // Get the current row's data as an item array
                    var obj = row.Row.ItemArray;
                    // Add the item of the current row at the current column's index and add it to the outbound list.
                    try1.Add(obj[thisC].ToString());
                }
            }

            var dic = new Dictionary<string, int>();
            for (int i = 0; i < try1.Count(); i += 2)
            {
                dic.Add(try1[i], int.Parse(try1[i + 1]));
            }

            var data = new FrequencyData<string>(dic);

            var saveChartToDialog = new SaveFileDialog
            {
                Filter = "JPeg Image|*.jpg",
                Title = "Save graph to..."
            };

            saveChartToDialog.ShowDialog(this);

            if (!string.IsNullOrWhiteSpace(saveChartToDialog.FileName))
            {
                _grapher.PlotBarChart(data, saveChartToDialog.FileName);
                ImageViewWindow BarChart = new(saveChartToDialog.FileName);
                BarChart.Show();
            }
            else
            {
                MessageBox.Show("Graph not saved.");
            }
        }

        private void xyButton_Click(object sender, RoutedEventArgs e)
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
                    var row = (DataRowView) cont.DataContext;
                    // Get the current row's data as an item array
                    var obj = row.Row.ItemArray;
                    // Add the item of the current row at the current column's index and add it to the outbound list.
                    try1.Add(obj[thisC].ToString());
                }
            }

            var saveChartToDialog = new SaveFileDialog
            {
                Filter = "JPeg Image|*.jpg",
                Title = "Save graph to..."
            };

            saveChartToDialog.ShowDialog(this);
            List<double> data = new();
            for (int i = 0; i < try1.Count; i++) data.Add(double.Parse(try1[i]));
            if (!string.IsNullOrWhiteSpace(saveChartToDialog.FileName))
            {
                _grapher.PlotXYGraph(data, saveChartToDialog.FileName);
                ImageViewWindow XYGraph = new(saveChartToDialog.FileName);
                XYGraph.Show();
            }
            else
            {
                MessageBox.Show("Graph not saved.");
            }
        }

        private void normalDistButton_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion

        private void SetParameters_OnClick(object sender, RoutedEventArgs e)
        {
            var parametersNeeded = statisticTypesToPerform
                .SelectMany(Utilities.GetParameterNames)
                .ToList();

            if (parametersNeeded.Count == 0)
            {
                MessageBox.Show("There are no statistics selected that require parameters!");
                return;
            }

            var parametersDialog = new ParameterDialog(parametersNeeded)
            {
                Owner = this
            };

            var accepted = parametersDialog.ShowDialog() ?? false;

            if (!accepted)
            {
                MessageBox.Show("Parameters were not accepted.");
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            new HelpDialog().ShowDialog();
        }
    }
}