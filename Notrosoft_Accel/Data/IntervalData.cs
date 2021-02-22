using System;
using System.Collections.Generic;
using System.Text;

namespace Notrosoft_Accel.Data
{
    /// <summary>
    /// Interval data is data that has order and differences between the two have meaning.
    /// </summary>
    /// <remarks>
    /// Examples would be the pH of a substance, SAT scores, and temperatures.
    /// </remarks>
    class IntervalData : GenericData<double>
    {
        public IntervalData(IEnumerable<IEnumerable<double>> data) : base(data, isInterval: true)
        {
        }
    }
}
