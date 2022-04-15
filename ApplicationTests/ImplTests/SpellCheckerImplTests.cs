using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApplication.Implementation;
using TestApplication.Interface;

namespace ApplicationTests.ImplTests
{
    public class SpellCheckerImplTests
    {
        private readonly Mock<IWriter> _writerMock = new Mock<IWriter>();
        private readonly Mock<IReader> _readerMock = new Mock<IReader>();
        private readonly Mock<IDistance> _distanceMock = new Mock<IDistance>();
        private ISpellChecker _spellChecker;
        [SetUp]
        public void SetUp()
        {
            _distanceMock.Setup(x => x.CalculateDistance(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(2);
            _readerMock.Setup(x => x.Read(It.IsAny<string>())).Returns((new LinkedList<string>(new[] { "glossary","list" }), new LinkedList<string[]>(new[] { new[] { "input" } })));
            _writerMock.Setup(x => x.Write(It.IsAny<IEnumerable<string[]>>(), It.IsAny<string>()));
            _spellChecker = new SpellCheckerImpl(_distanceMock.Object, _readerMock.Object, _writerMock.Object);
        }
        [Test]
        public void TestsForBadInput()
        {
            Assert.Throws<ArgumentNullException>(() => _spellChecker.CheckSpelling("", "abc", 2));
            Assert.Throws<ArgumentNullException>(() => _spellChecker.CheckSpelling(null, "abc", 2));
            Assert.Throws<ArgumentNullException>(() => _spellChecker.CheckSpelling("abc", "", 2));
            Assert.Throws<ArgumentNullException>(() => _spellChecker.CheckSpelling("abc", null, 2));
            Assert.Throws<ArgumentException>(() => _spellChecker.CheckSpelling("abc", "abc", -5));
            Assert.Throws<ArgumentException>(() => _spellChecker.CheckSpelling("abc", "abc", 0));
        }
        [Test]
        public void TestsForCorrectInput()
        {
            _spellChecker.CheckSpelling("abc", "abc", 2);
            _distanceMock.Verify(x => x.CalculateDistance(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(2));
            _writerMock.Verify(x => x.Write(It.IsAny<IEnumerable<string[]>>(), It.IsAny<string>()), Times.Once());
            _readerMock.Verify(x => x.Read(It.IsAny<string>()), Times.Once());
        }
    }
}
