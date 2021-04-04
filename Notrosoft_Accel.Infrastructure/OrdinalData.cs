using System.Collections.Generic;

namespace Notrosoft_Accel.Infrastructure
{
    public class OrdinalData : List<List<double>>
    {
        public IEnumerable<double> Flatten()
        {
            return Utilities.Flatten(this);
        }
    }
}