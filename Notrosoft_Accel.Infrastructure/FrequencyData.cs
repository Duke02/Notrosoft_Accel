using System.Collections.Generic;

namespace Notrosoft_Accel.Infrastructure
{
    public class FrequencyData<T> : Dictionary<T, int>
    {
        public FrequencyData(IDictionary<T, int> data) : base(data)
        {
        }
    }
}