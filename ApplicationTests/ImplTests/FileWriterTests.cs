using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using TestApplication.Implementation;

namespace ApplicationTests.ImplTests
{
    public class FileWriterTests
    {
        [Test]
        public void TestForCorrectInput()
        {
            var writer = new FileWriter();
            var list = new List<string[]>(new[] { new[] { "string" }, new[] { "string2" } });

            var path = "./Resources/output.txt";
            writer.Write(list, path);
            var linesFromFile = File.ReadAllLines(path);
            Assert.AreEqual(list[0][0], linesFromFile[0]);
            Assert.AreEqual(list[1][0], linesFromFile[1]);
        }
        [Test]
        public void TestsForBadInput()
        {
            var writer = new FileWriter();
            var list = new LinkedList<string[]>(new[] { new[] { "string" } });
            var emptyList = new LinkedList<string[]>();
            Assert.Throws<ArgumentNullException>(() => writer.Write(list, null));
            Assert.Throws<ArgumentNullException>(() => writer.Write(list, ""));
            Assert.Throws<ArgumentNullException>(() => writer.Write(null, "./Resources/output.txt"));
            Assert.Throws<ArgumentException>(() => writer.Write(emptyList, "./Resources/output.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => writer.Write(list, "./Resources/"));
        }
    }
}
