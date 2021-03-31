using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;
using System.Globalization;

namespace Notrosoft_Accel
{
    class FileInput
    {
        private List<List<string>> data;
        public List<List<string>> ReadFile() {
            string filePath = Console.ReadLine();
            data = new List<List<string>>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                while (csv.Read()) {
                    var records = csv.GetRecords<string>().ToList();
                    data.Add(records);
                }
            }
            return data;
        }
    }
}
