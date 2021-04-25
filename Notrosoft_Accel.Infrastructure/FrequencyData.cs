using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Infrastructure
{
    /// <summary>
    /// Keeps track of the number of times the key occurs within the dataset.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FrequencyData<T> : Dictionary<T, int>, INotrosoftData
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

                return frequencies.ToDictionary(fs => fs.Key,
                    fs => frequencies.TakeWhile(f => !f.Key.Equals(fs.Key)).Sum(f => f.Freq));
            }
        }

        public static IEnumerable<FrequencyData<double>> ConvertDouble(IEnumerable<IEnumerable<string>> data,
            int numColumns)
        {
            var concreteData = data.Select(l => l.ToArray()).ToArray();

            switch (numColumns)
            {
                case 2:
                {
                    var output = new Dictionary<double, int>();

                    // TODO: Handle it Cameron's way.
                    foreach (var row in concreteData)
                    {
                        var valueStr = row[0];
                        var countStr = row[1];

                        if (!double.TryParse(valueStr, out var value))
                        {
                            throw new InvalidOperationException("Cannot parse value of Frequency data!");
                        }

                        if (!int.TryParse(countStr, out var count))
                        {
                            throw new InvalidOperationException("Cannot parse count of frequency data!");
                        }

                        output[value] = count;
                    }

                    return new List<FrequencyData<double>>()
                    {
                        new FrequencyData<double>(output)
                    };
                }
                case 1:
                    return Utilities.ParseForDoubles(concreteData).Select(Utilities.ConvertToFrequencyValues);
                default:
                    throw new InvalidOperationException("Got wrong number of columns.");
            }
        }
    }
}