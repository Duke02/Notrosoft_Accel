using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Collections.ObjectModel;

namespace Notrosoft_Accel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int colNum = 10;  // Number of Columns present in the data structure
        public static int rowNum = 10;  // Number of Rows present in the data structure
        public static List<List<string>> dataList = new List<List<string>>();   // The data structure
        public static DataTable dataTable = new DataTable();  // DataTable sits between the dataList of the backend and DataGrid of the GUI

        public MainWindow()
        {
            // The List<String> representing the default column List of size colNum.
            List<string> colConstruct = new List<string>();

            for (int i = 0; i < colNum; i++)
            {
                colConstruct.Add("");
            }
            // Construct the rows as the empty column structure
            for (int i = 0; i < rowNum; i++)
            {
                dataList.Add(colConstruct);
            }

            InitializeComponent();
            Console.WriteLine("Hello World!");
        }

        // Constructs a dataTable object and binds it to the DataGrid's ItemsSource.
        void dataGridTable()
        {
            // Set the dataTable to a new object
            dataTable = new DataTable();

            // Column header constructor
            char currentNameCharacter = 'A';                // The character that determines the name of the column
            int iteration = 1;                              // The current iteration through the uppercase letters 
            String newColName = ("" + currentNameCharacter);// The constructed first name (should simply by "A")

            // Column constructor
            for (int i = 0; i < colNum; i++)
            {
                // Instantiate a new DataColumn type
                DataColumn newColumn = new DataColumn(newColName, typeof(string));
                if (currentNameCharacter < 'Z') currentNameCharacter++;
                else
                {
                    currentNameCharacter = 'A';
                    iteration++;
                }
                newColName = ("");
                for (int j = 0; j < iteration; j++)
                {
                    newColName += currentNameCharacter;
                }
                
                dataTable.Columns.Add(newColumn);
            }
            
            for (int i = 0; i < rowNum; i++)
            {
                DataRow newRow = dataTable.NewRow();
                for (int j = 0; j < colNum; j++)
                {
                    newRow[j] = dataList[i][j];
                }                
                dataTable.Rows.Add(newRow);
            }
            
            Data.ItemsSource = dataTable.DefaultView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridTable();
        }

        private void addColumnButton_Click(object sender, RoutedEventArgs e)
        {
            
            
            for (int i = 0; i < rowNum; i++)
            {
                List<string> updatedRow = new List<string>();
                for (int j = 0; j < colNum; j++)
                {
                    updatedRow.Add(dataTable.Rows[i][j].ToString());
                }
                dataList[i] = updatedRow;
            }
            colNum++;

            for (int i = 0; i < rowNum; i++)
            {
                dataList[i].Add("");
            }
            dataGridTable();
        }
    }
}
