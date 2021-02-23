using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Notrosoft_Accel.Data
{
    /// <summary>
    /// The generic data class that helps allow for different data types to be used almost interchangably.
    /// </summary>
    /// <typeparam name="T">The type of the actual data that is being represented.</typeparam>
    public class GenericData<T>
    {
        /// <summary>
        /// The actual data that this object represents. Immutable unless changed by the user outside of the backend.
        /// </summary>
        public IEnumerable<IEnumerable<T>> Data { get; protected set; }

        /// <summary>
        /// True if the data is castegorical and is ordered. False otherwise.
        /// </summary>
        public bool IsOrdinal { get; protected set; }

        /// <summary>
        /// True if the data counts the number of times an event occurs. False otherwise.
        /// </summary>
        public bool IsFrequency { get; protected set; }

        /// <summary>
        /// True if the data is based on ranges of data that is ordered and its categories have differences in meaning between each other. False otherwise.
        /// </summary>
        public bool IsInterval { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">The data to be stored and represented.</param>
        /// <param name="isOrdinal">True if the data is categorical and ordered.</param>
        /// <param name="isFrequency">True if the data counts the occurences of an event.</param>
        /// <param name="isInterval">True if the data is ordered and has differences in meanings between the intervals.</param>
        protected GenericData(IEnumerable<IEnumerable<T>> data, bool isOrdinal = false, bool isFrequency = false, bool isInterval = false)
        {
            Data = data;
            IsOrdinal = isOrdinal;
            IsFrequency = isFrequency;
            IsInterval = isInterval;
        }

        /// <summary>
        /// Gets the data at the given row and column.
        /// </summary>
        /// <param name="row">The first index of the data point requested.</param>
        /// <param name="col">The second index of the data point requested.</param>
        /// <returns>The data at the given point.</returns>
        /// <exception cref="IndexOutOfRangeException">Throws this exception when the given indexes are out of bounds.</exception>
        public T GetData(int row, int col)
        {
            return Data.ElementAt(row).ElementAt(col);
        }

        /// <summary>
        /// Gets the data at the given row and column.
        /// </summary>
        /// <param name="row">The first index of the data point requested.</param>
        /// <param name="col">The second index of the data point requested.</param>
        /// <returns>The data at the given point.</returns>
        /// <exception cref="IndexOutOfRangeException">Throws this exception when the given indexes are out of bounds.</exception>
        public T this[int row, int col]
        {
            get => GetData(row, col);
        }

        public IEnumerable<T> Flatten()
        {
            return Data.SelectMany(vals => vals);
        }
    }
}
