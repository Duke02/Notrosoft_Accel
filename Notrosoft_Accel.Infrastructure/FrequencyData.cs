using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Infrastructure
{
    /// <summary>
    /// Keeps track of the number of times the key occurs within the dataset.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FrequencyData<T> : Dictionary<T, int>
    {
        public FrequencyData(IDictionary<T, int> data) : base(data)
        {
        }

        public int TotalSize => Values.Sum(v => v);
    }
}