using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Notrosoft_Accel.Annotations;

namespace Notrosoft_Accel
{
    public partial class ParameterDialog : Window
    {
        public ParameterDialog(List<string> parametersNeeded)
        {
            InitializeComponent();

            for (var i = 0; i < parametersNeeded.Count; i++)
            {
                Grid.RowDefinitions.Insert(i, new RowDefinition() {Height = GridLength.Auto});
                var row = new ParameterRow(parametersNeeded[i])
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch
                };
                Grid.SetRow(row, i);
                Grid.SetColumn(row, 0);
                Grid.SetColumnSpan(row, 3);
                Grid.Children.Add(row);
            }

            Grid.RowDefinitions.Add(new RowDefinition() {Height = GridLength.Auto});
            
            Grid.SetRow(OkButton, parametersNeeded.Count);
            Grid.SetRow(CancelButton, parametersNeeded.Count);

            // ParameterPanel.UpdateLayout();
            Grid.UpdateLayout();
            
            // this.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        }

        private void ParameterDialog_OnClosing(object sender, CancelEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Owner is not MainWindow mainWindow)
            {
                throw new Exception("Error casting owner of parameter window as the main window!");
            }

            var output = new Dictionary<string, double>();

            foreach (UIElement paramRow in Grid.Children)
            {
                if (paramRow is not ParameterRow row)
                {
                    continue;
                }

                var name = row.ParameterName;

                if (!double.TryParse(row.ParameterValue, out var value))
                {
                    MessageBox.Show(Owner, $"Cannot parse value for parameter {name}!", "Error parsing parameter!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    DialogResult = false;
                    return;
                }

                output.Add(name, value);
            }

            mainWindow.Parameters = output;
            DialogResult = true;
        }
    }
}