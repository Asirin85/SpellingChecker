using System;
using TestApplication.Implementation;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var distance = new LevenshteinDistance();
            var reader = new FileReader();
            var writer = new FileWriter();
            var spellChecker = new SpellCheckerImpl(distance, reader, writer);
            Console.WriteLine("Please write your input path");
            Console.WriteLine("Example: C:/Users/username/file.txt");
            Console.Write("> ");
            var inputPath = Console.ReadLine();
            Console.WriteLine("Please write your output path: ");
            Console.WriteLine("Example: C:/Users/username/file.txt");
            Console.Write("> ");
            var outputPath = Console.ReadLine();
            spellChecker.CheckSpelling(inputPath, outputPath, 2);
            Console.WriteLine($"Result was printed to {outputPath}");
        }
    }
}
