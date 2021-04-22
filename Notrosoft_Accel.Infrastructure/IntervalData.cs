using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Infrastructure
{
    public class IntervalData : Dictionary<string, IEnumerable<double>>
    {
        public IntervalData(Dictionary<string, IEnumerable<double>> data) : base(data)
        {
            Definitions = new IntervalDefinitions(data);
        }

        public IntervalDefinitions Definitions { get; }

        public static IntervalData GetIntervalData(List<double> flattenedData, IntervalDefinitions definitions)
        {
            return Utilities.ConvertToIntervalData(flattenedData, definitions);
        }

        public Dictionary<string, int> Frequencies => this.ToDictionary(kv => kv.Key, kv => kv.Value.Count());

        public Dictionary<string, int> CumulativeFrequencies
        {
            get
            {
                var output = new Dictionary<string, int>();
                var orderedDefs = Definitions.OrderBy(kv => kv.Value.Start).ToList();

                var frequencies = Frequencies.Join(orderedDefs, kv => kv.Key,
                    kv => kv.Key,
                    (freq, def) => new
                    {
                        Freq = freq.Value,
                        Start = def.Value.Start,
                        Name = def.Key
                    })
                    .OrderBy(fs => fs.Start)
                    .ToList();

                return frequencies.ToDictionary(fs => fs.Name, fs => frequencies.TakeWhile(f => f.Name != fs.Name).Sum(f => f.Freq));
            }
        }

        public int TotalSize => Frequencies.Sum(kv => kv.Value);
    }
}