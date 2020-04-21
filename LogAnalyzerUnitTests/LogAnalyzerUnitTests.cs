using NUnit.Framework;
using System;
using TestKatas1;
using TestKatas1.Interfaces;
using FakeItEasy;

namespace LogAnalyzerUnitTests
{
    [TestFixture]
    public class LogAnalyzerUnitTests
    {
        private IExtensionManager _manager;

        [SetUp]
        public void Setup()
        {
            _manager = A.Fake<IExtensionManager>();
        }

        [TestCase("filewithbadextension.foo", false)]
        [TestCase("filewithgoodextension.slf", true)]
        [TestCase("filewithgoodextension.SLF", true)]
        public void IsValidFileName_BadExtension_ReturnsFalse(string filename, bool expected)
        {
            var _analyzer = MakeAnalyzer(_manager);
            bool result = _analyzer.IsValidLogFileName(filename);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void IsValidFileName_EmptyFileName_Throws()
        {
            var _analyzer = MakeAnalyzer(_manager);
            var ex = Assert.Catch<Exception>(() => _analyzer.IsValidLogFileName(""));
            
            StringAssert.Contains("filename has to be provided", ex.Message);
        }

        private LogAnalyzer MakeAnalyzer(IExtensionManager mgr)
        {
            return new LogAnalyzer(mgr);
        }
    }
}
