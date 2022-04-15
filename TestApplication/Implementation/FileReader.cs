using System;
using System.Collections.Generic;
using System.Linq;
using TestApplication.Interface;

namespace TestApplication.Implementation
{
    public class FileReader : IReader
    {
        /// <summary>
        /// Method for reading and parsing received glossary and input lines
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public (IEnumerable<string> glossary, IEnumerable<string[]> input) Read(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path), "Input path is empty");
            var lines = System.IO.File.ReadAllLines(path);
            if (lines.Length == 0) throw new ArgumentNullException(nameof(lines), "Input file is empty");
            var glossary = new LinkedList<string>();
            var i = 0;
            while (i < lines.Length && !lines[i].Equals("==="))
            {
                ReadWordsFromLine(glossary, lines[i]);
                i++;
            }
            if (glossary.Count == 0) throw new ArgumentException("Input doesn't contain a dictionary", nameof(glossary));
            var input = new LinkedList<string[]>();
            if (i < lines.Length && lines[i].Equals("==="))
            {
                i++;
                while (i < lines.Length && !lines[i].Equals("==="))
                {
                    ReadWordsFromLine(input, lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries));
                    i++;
                }
            }
            if (input.Count == 0) throw new ArgumentException("Input doesn't contain words for spell checking", nameof(input));
            if (i == lines.Length) throw new ArgumentException("Bad EoF. File must end with === symbol", nameof(lines));
            return (glossary, input);
        }

        private void ReadWordsFromLine(LinkedList<string> readTo, string line)
        {
            if (readTo is null) throw new ArgumentNullException(nameof(readTo), "Collection can not be null");
            if (line is null) throw new ArgumentNullException(nameof(readTo), "Line to read from can not be null");
            var elements = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (elements.Any(x => x.Length > 50)) throw new ArgumentException("Input contains a word longer than 50 characters", nameof(line));
            foreach (var element in elements)
            {
                readTo.AddLast(element);
            }
        }
        private void ReadWordsFromLine(LinkedList<string[]> readTo, string[] line)
        {
            if (readTo is null) throw new ArgumentNullException(nameof(readTo), "Collection can not be null");
            if (line is null) throw new ArgumentNullException(nameof(readTo), "Line to read from can not be null");
            if (line.Any(x => x.Length > 50)) throw new ArgumentException("Input contains a word longer than 50 characters", nameof(line));
            readTo.AddLast(line);
        }
    }
}
