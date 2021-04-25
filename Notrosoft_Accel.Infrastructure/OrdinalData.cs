using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Infrastructure
{
    public class OrdinalData : List<double>, INotrosoftData
    {
        public OrdinalData(IEnumerable<double> data) : base(data)
        {
        }

        public OrdinalData() : base()
        {
            
        }
        
        public static IEnumerable<OrdinalData> Convert(IEnumerable<IEnumerable<string>> data, int numColumns)
        {
            return Utilities.ParseForDoubles(data)
                .Select(l => new OrdinalData(l))
                .ToList();
        }
    }
}