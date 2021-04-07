using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Infrastructure
{
    public class IntervalDefinitions : Dictionary<string, Bounds>
    {
        public IntervalDefinitions()
        {
        }

        public IntervalDefinitions(Dictionary<string, IEnumerable<double>> data) : base(
            data.ToDictionary(kv => kv.Key,
                kv => new Bounds(kv.Value)))
        {
        }
    }
}