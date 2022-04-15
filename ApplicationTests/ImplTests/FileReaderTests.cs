using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TestApplication.Implementation;

namespace ApplicationTests.ImplTests
{
    public class FileReaderTests
    {
        private readonly string path = "./Resources/correctData.txt";

        [Test]
        public void TestForCorrectInput()
        {
            var expectedGlossary = new LinkedList<string>(new[] { "main", "pain", "drain", "line", "game", "lame", "a" });
            var expectedInput = new LinkedList<string[]>(new[] { new[] { "life", "is", "ai", "GAME", "drawn", "by", "shame", "pabibn", "ae" }, new[] { "woopsie", "doopsie", "carloopsie", "lain" } });
            var reader = new FileReader();
            var result = reader.Read(path);
            Assert.That(result.glossary.Count, Is.EqualTo(expectedGlossary.Count));
            Assert.That(result.input.Count, Is.EqualTo(expectedInput.Count));
            CollectionAssert.AreEqual(expectedGlossary, result.glossary);
            CollectionAssert.AreEqual(expectedInput, result.input);
        }
        [Test]
        public void TestsForBadInput()
        {
            var reader = new FileReader();
            Assert.Throws<ArgumentNullException>(() => reader.Read(null));
            Assert.Throws<ArgumentNullException>(() => reader.Read(""));
            Assert.Throws<ArgumentNullException>(() => reader.Read("./Resources/empty.txt"));
            Assert.Throws<ArgumentException>(() => reader.Read("./Resources/noGlossary.txt"));
            Assert.Throws<ArgumentException>(() => reader.Read("./Resources/noSpell.txt"));
            Assert.Throws<ArgumentException>(() => reader.Read("./Resources/badEnding.txt"));
            Assert.Throws<ArgumentException>(() => reader.Read("./Resources/longWord.txt"));
        }
    }
}
