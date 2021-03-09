using System.Collections.Generic;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     The base interface for statistics that all statistics are to inherit from.
    /// </summary>
    public abstract class Statistic
    {
        /// <summary>
        ///     Base method that all statistics are to override to use for their statistics.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <returns>The statistic for the child class.</returns>
        public abstract double Operate(IEnumerable<IEnumerable<double>> values);

        public virtual Dictionary<string, double> GetAdditionalParameters()
        {
            return new();
        }
    }
}