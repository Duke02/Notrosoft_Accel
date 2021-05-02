using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Notrosoft_Accel.Annotations;

namespace Notrosoft_Accel
{
    public partial class ParameterRow : UserControl, INotifyPropertyChanged
    {
        private string _paramName;
        private string _paramValue;

        public ParameterRow(string paramName)
        {
            ParameterName = paramName;
            DataContext = this;
            InitializeComponent();
        }

        public string ParameterName
        {
            get => _paramName;
            set
            {
                if (value == null || _paramName == value) return;
                _paramName = value;
                OnPropertyChanged();
            }
        }

        public string ParameterValue
        {
            get => _paramValue;
            set
            {
                if (value == null || _paramValue == value) return;
                _paramValue = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged is { } propHandler)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                propHandler(this, e);
            }
        }
    }
}