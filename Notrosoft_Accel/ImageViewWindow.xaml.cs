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
using System.Windows.Shapes;

namespace Notrosoft_Accel
{
    /// <summary>
    /// Interaction logic for ImageViewWindow.xaml
    /// </summary>
    public partial class ImageViewWindow : Window
    {
        BitmapImage bmap = new();
        public ImageViewWindow(string input)
        {
            bmap.BeginInit();
            bmap.UriSource = new Uri(input);
            bmap.EndInit();
            InitializeComponent();
            mWind.Title = input;
        }

        private void Graph1_Initialized(object sender, EventArgs e)
        {
            Graph1.Source = bmap;
        }
    }
}
