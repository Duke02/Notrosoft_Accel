using System;
using System.Collections.Generic;
using System.Text;

namespace Notrosoft_Accel.Data
{
    /// <summary>
    /// Frequency Data is data that counts the number of times an event occurs.
    /// </summary>
    /// <remarks>
    /// TODO: Ask Delugach if we need to keep track of which event that is? And just general clarification on what he's expecting for this data.
    /// </remarks>
    class FrequencyData : GenericData<int>
    {
        public FrequencyData(IEnumerable<IEnumerable<int>> data) : base(data, isFrequency: true)
        {
        }
    }
}
