using System;
using System.Collections.Generic;
using System.Text;

namespace Notrosoft_Accel.Data
{
    /// <summary>
    /// Ordinal data is categorical data where order matters. Stuff like income ranges, Educational level, etc.
    /// </summary>
    public class OrdinalData : GenericData<string>
    {
        public OrdinalData(IEnumerable<IEnumerable<string>> data) : base(data, isOrdinal: true)
        {
        }
    }
}
