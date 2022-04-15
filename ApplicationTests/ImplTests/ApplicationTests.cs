using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApplication.Implementation;

namespace ApplicationTests.ImplTests
{
    public class ApplicationTests
    {
        private const string inputPath = "./Resources/ApplicationTestsData/input.txt";
        private const string outputPath = "./Resources/ApplicationTestsData/output.txt";
        [Test]
        public void TestForCorrectSpellChecking()
        {
            var distance = new LevenshteinDistance();
            var reader = new FileReader();
            var writer = new FileWriter();
            var spellChecker = new SpellCheckerImpl(distance, reader, writer);
            spellChecker.CheckSpelling(inputPath, outputPath, 2);
            var expectedResult = new string[] { "line {is?} a GAME drain {by?} {shame?} pain a", "{wpsie?} woopsie {doopbsbie woopsie} {carloopsie?} {main pain line}" };
            var linesFromFile = File.ReadAllLines(outputPath);
            Assert.AreEqual(expectedResult.Length, linesFromFile.Length);
            Assert.AreEqual(expectedResult[0], linesFromFile[0]);
            Assert.AreEqual(expectedResult[1], linesFromFile[1]);
        }
    }
}
