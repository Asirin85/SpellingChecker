using NUnit.Framework;
using System;
using TestApplication.Implementation;
namespace ApplicationTests.ImplTests
{
    public class LevenshteinDistanceTests
    {
        [Test]
        public void TestsForCorrectInput()
        {
            var distance = new LevenshteinDistance();
            Assert.AreEqual(1, distance.CalculateDistance("a", "ab"));
            Assert.AreEqual(1, distance.CalculateDistance("ab", "a"));
            Assert.AreEqual(-1, distance.CalculateDistance("a", "abc"));
            Assert.AreEqual(-1, distance.CalculateDistance("abc", "a"));
            Assert.AreEqual(2, distance.CalculateDistance("abc", "avbvc"));
            Assert.AreEqual(2, distance.CalculateDistance("avbvc", "abc"));
            Assert.AreEqual(3, distance.CalculateDistance("avbvcv", "abc", 3));
            Assert.AreEqual(-1, distance.CalculateDistance("avbvvc", "abc", 3));
        }
        [Test]
        public void TestsForBadInput()
        {
            var distance = new LevenshteinDistance();
            Assert.Throws<ArgumentNullException>(() => distance.CalculateDistance(null, ""));
            Assert.Throws<ArgumentNullException>(() => distance.CalculateDistance("", null));
            Assert.Throws<ArgumentException>(() => distance.CalculateDistance("", "", 0));
            Assert.Throws<ArgumentException>(() => distance.CalculateDistance("", "", -10));
        }
    }
}
