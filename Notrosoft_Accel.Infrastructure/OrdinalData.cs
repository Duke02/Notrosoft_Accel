using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Infrastructure
{
    public class OrdinalData : List<List<double>>
    {
        public OrdinalData(IEnumerable<IEnumerable<double>> data) : base(data.Select(d => d.ToList()))
        {
        }

        public IEnumerable<double> Flatten()
        {
            return Utilities.Flatten(this);
        }
    }
}