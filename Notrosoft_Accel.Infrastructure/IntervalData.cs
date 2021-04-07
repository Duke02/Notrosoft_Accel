using System.Collections.Generic;

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
    }
}