using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestApplication.Interface;

namespace TestApplication.Implementation
{
    public class FileWriter : IWriter
    {
        /// <summary>
        /// Method for concating words to a line and then writing them to a file
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public void Write(IEnumerable<string[]> data, string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path), "Path is empty");
            if (data is null) throw new ArgumentNullException(nameof(data), "Output data is null");
            var amountOfLines = data.Count();
            if (amountOfLines == 0) throw new ArgumentException(nameof(data), "Output data is empty");
            var lines = new string[amountOfLines];
            var index = 0;
            foreach (var line in data)
            {
                var builder = new StringBuilder();
                foreach (var element in line)
                {
                    builder.Append($"{element} ");
                }
                lines[index++] = builder.ToString().Trim();
            }
            File.WriteAllLines(path, lines);
        }
    }
}
