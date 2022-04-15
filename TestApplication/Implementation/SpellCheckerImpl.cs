using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApplication.Interface;

namespace TestApplication.Implementation
{
    public class SpellCheckerImpl : ISpellChecker
    {
        private readonly IDistance _distance;
        private readonly IReader _reader;
        private readonly IWriter _writer;
        public SpellCheckerImpl(IDistance distance, IReader reader, IWriter writer)
        {
            _distance = distance;
            _reader = reader;
            _writer = writer;
        }
        public void CheckSpelling(string inputPath, string outputPath, int errorThreshold = 2)
        {
            if (string.IsNullOrEmpty(inputPath)) throw new ArgumentNullException(nameof(inputPath), "Input path is empty");
            if (string.IsNullOrEmpty(outputPath)) throw new ArgumentNullException(nameof(outputPath), "Input path is empty");
            if (errorThreshold <= 0) throw new ArgumentException("Error threshold must be set more than 0", nameof(errorThreshold));
            var (glossary, input) = _reader.Read(inputPath);
            var glossaryLength = glossary.Count();
            foreach (var line in input)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    string word = line[i];
                    var oneError = new LinkedList<string>();
                    var twoErrors = new LinkedList<string>();
                    var foundCorrect = false;
                    for (int k = 0; k < glossaryLength && !foundCorrect; k++)
                    {
                        var element = glossary.ElementAt(k);
                        int res;
                        if (oneError.Count > 0)
                            res = _distance.CalculateDistance(element, word, 1);
                        else res = _distance.CalculateDistance(element, word, errorThreshold);
                        if (res == 0) foundCorrect = true;
                        else if (res == 1) { oneError.AddLast(element); }
                        else if (res == 2) { twoErrors.AddLast(element); }
                    }
                    if (!foundCorrect)
                    {
                        line[i] = CreateResultingWord(oneError, twoErrors, line[i]);
                    }
                }
            }
            _writer.Write(input, outputPath);
        }
        /// <summary>
        /// Method for changing resulting words into format required by task
        /// </summary>
        /// <param name="oneError"></param>
        /// <param name="twoErrors"></param>
        /// <param name="initialWord"></param>
        /// <returns></returns>
        private string CreateResultingWord(LinkedList<string> oneError, LinkedList<string> twoErrors, string initialWord)
        {
            if (oneError is null) throw new ArgumentNullException(nameof(oneError), "A list that contains words with one misspelling is null");
            if (twoErrors is null) throw new ArgumentNullException(nameof(twoErrors), "A list that contains words with two misspellings is null");
            if (initialWord is null) throw new ArgumentNullException(nameof(initialWord), "Initial word is empty");
            var builder = new StringBuilder();
            if (oneError.Count > 0)
            {
                if (oneError.Count > 1)
                {
                    builder.Append("{");
                    foreach (var word in oneError)
                    {
                        builder.Append($"{word} ");
                    }
                    builder[builder.Length - 1] = '}';
                }
                else builder.Append($"{oneError.First.Value}");
            }
            else if (twoErrors.Count > 0)
            {
                if (twoErrors.Count > 1)
                {
                    builder.Append("{");
                    foreach (var word in twoErrors)
                    {
                        builder.Append($"{word} ");
                    }
                    builder[builder.Length - 1] = '}';
                }
                else builder.Append($"{twoErrors.First.Value}");
            }
            else builder.Append($"{{{initialWord}?}}");
            return builder.ToString();
        }
    }
}
