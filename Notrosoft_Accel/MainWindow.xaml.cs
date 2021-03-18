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
        public static int colNum = 10;
        public static int rowNum = 10;
        public static List<List<string>> dataList = new List<List<string>>();
        

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

        void dataGridTable()
        {
            DataTable dataTable = new DataTable();


            // Column header constructor
            char name = 'A';
            int iteration = 1;
            String nS = ("" + name);
            // Column constructor
            for (int i = 0; i < colNum; i++)
            {
                DataColumn newColumn = new DataColumn(nS, typeof(string));
                if (name < 'Z') name++;
                else
                {
                    name = 'A';
                    iteration++;
                }
                nS = ("");
                for (int j = 0; j < iteration; j++)
                {
                    nS += name;
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
            DataRow nR = dataTable.NewRow();
            nR[0] = dataList[0][0];
            nR[1] = dataList[0][0];
            dataTable.Rows.Add(nR);
            Data.DataContext = dataTable.DefaultView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridTable();
        }

        private void addColumnButton_Click(object sender, RoutedEventArgs e)
        {
            
            
            colNum++;

            for (int i = 0; i < rowNum; i++)
            {
                dataList[i].Add("");
            }
            dataGridTable();
        }
    }
}
