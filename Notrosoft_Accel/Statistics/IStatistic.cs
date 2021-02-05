using System;
using System.Collections.Generic;
using System.Text;

namespace Notrosoft_Accel.Statistics
{
    public interface IStatistic
    {
        public double Operate(IEnumerable<IEnumerable<double>> values);
    }
}
