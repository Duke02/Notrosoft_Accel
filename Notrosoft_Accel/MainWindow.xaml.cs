using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace Notrosoft_Accel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int colNum = 10;
        public MainWindow()
        {
            List<List<double>> arr2d = new List<List<double>>();
            List<double> rows = new List<double>();
            rows.Add(12.1);
            rows.Add(13.1992);
            arr2d.Add(rows);
            arr2d.Add(rows);

            InitializeComponent();
            Console.WriteLine("Hello World!");
        }
        void dataGridTable()
        {
            DataTable notData = new DataTable();

            char name = 'a';
            String nS = ("" + name);
            for (int i = 0; i < colNum; i++)
            {
                DataColumn c1 = new DataColumn(nS, typeof(double));
                name++;
                nS = ("" + name);
                notData.Columns.Add(c1);
            }
            
            DataRow r1 = notData.NewRow();
            
            r1[0] = 12.1;
            r1[1] = 13.1992;
            r1[5] = 0.01;
            r1[6] = 101;

            DataRow r2 = notData.NewRow();
            r2[0] = 1313.3131;
            r2[1] = 0.922388811;

            notData.Rows.Add(r1);
            notData.Rows.Add(r2);

            
            Data.ItemsSource = notData.DefaultView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridTable();
        }
    }
}
