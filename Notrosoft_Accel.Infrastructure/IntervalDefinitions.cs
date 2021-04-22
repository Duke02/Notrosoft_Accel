using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Infrastructure
{
    public class IntervalDefinitions : Dictionary<string, Bounds>
    {
        public IntervalDefinitions()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">The data for each interval with the key being the name of it, and the value being the actual data of the interval.</param>
        public IntervalDefinitions(Dictionary<string, IEnumerable<double>> data) : base(
            data.ToDictionary(kv => kv.Key,
                kv => new Bounds(kv.Value)))
        {
        }

        public IEnumerable<double> Midpoints => Values.Select(b => b.GetMidpoint());

        public IEnumerable<double> Sizes => Values.Select(b => b.Size);

        public (string, Bounds) GetBoundForValue(double value)
        {
            var output = this.First(nb => nb.Value.IsWithinBounds(value));
            return (output.Key, output.Value);
        }
    }
}