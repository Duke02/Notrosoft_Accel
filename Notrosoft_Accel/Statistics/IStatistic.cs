using Notrosoft_Accel.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notrosoft_Accel.Statistics
{
    /// <summary>
    /// The base interface for statistics that all statistics are to inherit from.
    /// </summary>
    public interface IStatistic
    {
        /// <summary>
        /// Base method that all statistics are to override to use for their statistics.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <returns>The statistic for the child class.</returns>
        public object Operate<T>(GenericData<T> values);
    }
}
