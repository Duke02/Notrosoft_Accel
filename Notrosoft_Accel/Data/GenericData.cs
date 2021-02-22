using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Notrosoft_Accel.Data
{
    public class GenericData<T>
    {
        public IEnumerable<IEnumerable<T>> Data { get; protected set; }

        public bool IsOrdinal { get; protected set; }

        public bool IsFrequency { get; protected set; }

        public bool IsInterval { get; protected set; }

        protected GenericData(IEnumerable<IEnumerable<T>> data, bool isOrdinal = false, bool isFrequency = false, bool isInterval = false)
        {
            Data = data;
            IsOrdinal = isOrdinal;
            IsFrequency = isFrequency;
            IsInterval = isInterval;
        }

        public T GetData(int row, int col)
        {
            return Data.ElementAt(row).ElementAt(col);
        }

        public T this[int row, int col]
        {
            get => GetData(row, col);
        }
    }
}
