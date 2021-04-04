using System.Collections.Generic;

namespace Notrosoft_Accel.Infrastructure
{
    public class IntervalData : Dictionary<string, IEnumerable<double>>
    {
        public static IntervalData GetIntervalData(List<double> flattenedData, IntervalDefinitions definitions)
        {
            return Utilities.ConvertToIntervalData(flattenedData, definitions);
        }
    }
}