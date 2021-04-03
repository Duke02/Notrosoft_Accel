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
        public List<List<string>> ReadFile(string file) {
            data = new List<List<string>>();
            string[] allData = File.ReadAllLines(file);
            foreach (string line in allData)
            {
                var temp = new List<string>();
                string[] words = line.Split(", ");
                for (int i = 0; i < words.Length; i++)
                {
                    temp.Add(words[i]);
                }
                data.Add(temp);
            }
            return data;
        }
    }
}
