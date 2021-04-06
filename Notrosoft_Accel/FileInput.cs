using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Notrosoft_Accel
{
    internal class FileInput
    {
        /// <summary>
        ///     Reads the given CSV file and outputs a list of lists in row major order.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public List<List<string>> ReadFile(string file)
        {
            if (string.IsNullOrWhiteSpace(file)) throw new InvalidOperationException("File path must be filled in!");

            if (!File.Exists(file))
                throw new InvalidOperationException("Cannot import from a file that doesn't exist!");

            return File.ReadAllLines(file)
                .Select(line => line.Split(",").ToList())
                .ToList();
        }
    }
}