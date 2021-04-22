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

        public Dictionary<T, int> CumulativeFrequencies
        {
            get
            {
                // Taken from Interval data.
                var output = new Dictionary<string, int>();
                var orderedDefs = this.OrderBy(kv => kv.Key).ToList();

                var frequencies = this.Join(orderedDefs, kv => kv.Key,
                    kv => kv.Key,
                    (freq, def) => new
                    {
                        Freq = freq.Value,
                        Key = def.Key
                    })
                    .OrderBy(fs => fs.Key)
                    .ToList();

                return frequencies.ToDictionary(fs => fs.Key, fs => frequencies.TakeWhile(f => !f.Key.Equals(fs.Key)).Sum(f => f.Freq));
            }
        }

    }
}