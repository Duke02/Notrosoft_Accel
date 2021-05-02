using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Notrosoft_Accel.Annotations;

namespace Notrosoft_Accel
{
    public partial class HelpDialog : Window, INotifyPropertyChanged
    {
        private string _text = string.Empty;
        
        public HelpDialog()
        {
            DataContext = this;
            LoadHelpText();
            InitializeComponent();
        }

        public string HelpText
        {
            get => _text;
            set
            {
                if (value == null || _text == value) return;
                _text = value;
                OnPropertyChanged();
            }
        }
        
        private void LoadHelpText()
        {
            HelpText = @"Adding a Statistic

To add a statistic, a developer needs to create a C# class in the Notrosoft_Accel.Backend.Statistics namespace and inherit from the IStatistic interface. The custom statistic must implement its operations per the Ordinal, Interval, and Frequency data types as the statistic allows. If a statistic does not support a data type, the appropriate function must throw an InvalidOperationException that says “<Statistic Name> does not support <data type> data!”. This will pop up an error dialog whenever the user improperly selects the statistic to be performed on the incorrect data. Next, the developer must add the statistic type to the Notrosoft_Accel.Infrastructure.StatisticsType enumeration. The developer must edit the switch statement in Notrosoft_Accel.Interlayer#GetStatistic function so that it handles the new statistic appropriately. If there are any parameters needed for the statistic, the developer must also edit the Notrosoft_Accel.Infrastructure.Utilities#GetParameterNames function to add the parameter names to the output list. This is to ensure that the parameters input dialog appears when the user selects the statistic. Finally, the user must add the checkbox for the statistic in the statistic panel in the Notrosoft_Accel.MainWindow XAML file and handle the cases for checking and unchecking the checkbox. 

========================

Inputting Interval Definitions

To input an interval, the user must input the definition in the data grid. It must be in the format of 
Interval 1 Name | Interval 1 Start | Interval 1 Exclusive End
Interval 2 Name | Interval 2 Start | Interval 1 Exclusive End

And so on for each interval  the user needs to define. 

Next, the user must select the cells the interval definitions are in and click on the Set Intervals button. Now, the definitions will be used in all future statistic operations until this process is performed again.
";
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}