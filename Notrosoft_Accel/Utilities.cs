using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notrosoft_Accel
{
    public static class Utilities
    {
        public static IEnumerable<T> Flatten<T>(IEnumerable<IEnumerable<T>> values)
        {
            return values.SelectMany(vals => vals);
        }
    }
}
