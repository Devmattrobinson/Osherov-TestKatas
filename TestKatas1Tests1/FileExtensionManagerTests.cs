using NUnit.Framework;
using System;
using TestKatas1;
using TestKatas1.Interfaces;
using FakeItEasy;
using NuGet.Frameworks;
using TestKatas1.Models;
using TestKatas1.Services;

namespace LogAnalyzerUnitTests
{
    [TestFixture]
    public class LogAnalyzerUnitTests
    {
        private IFileRulesManager _fileRulesManager;
        private ILoggerService _logger;
        private IEmailService _emailService;

        [SetUp]
        public void Setup()
        {
            _fileRulesManager = A.Fake<FileRulesManager>();
            _logger = A.Fake<LoggerService>();
            _emailService = A.Fake<EmailService>();

        }

        [TestCase("filewithbadextension.foo", false)]
        [TestCase("filewithgoodextension.slf", true)]
        [TestCase("filewithgoodextension.SLF", true)]
        public void IsValidFileName_BadExtension_ReturnsFalse(string filename, bool expected)
        {
            bool result = _fileRulesManager.IsValidExtension(filename);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void IsValidFileName_EmptyFileName_Throws()
        {
            var analyzer = MakeAnalyzer(_logger, _emailService);
            var ex = Assert.Catch<Exception>(() => analyzer.ProcessFile(""));

            StringAssert.Contains("filename has to be provided", ex.Message);
        }

        [Test]
        public void Analyze_TooShortFileName_CallsLoggerService()
        {
            var logger = A.Fake<ILoggerService>();
            var analyzer = MakeAnalyzer(logger, _emailService);
            string tooShortFileName = "a.ext";
            analyzer.ProcessFile(tooShortFileName);
            A.CallTo(() => logger.LogError($"Filename too short: {tooShortFileName}")).MustHaveHappened();  // Same as the Received method from NSub Api
        }

        [Test]
        public void Analyze_FakeWebservice_SendEmail()
        {
            var stubService = new FakeWebService
            {
                ToThrow = new Exception("fake exception")
            };
            var emailService = new EmailService();
            var analyzer = MakeAnalyzer(stubService, emailService);
            string tooShortFileName = "abc.ext";

            analyzer.ProcessFile(tooShortFileName);

            //Assert.AreEqual(expectedEmail, emailService.Email);
            StringAssert.Contains("email1@address.com", emailService.Email.To);
            StringAssert.Contains("fake exception", emailService.Email.Body);
            StringAssert.Contains("Can't log", emailService.Email.Subject);
        }

        [Test]
        public void Analyze_logger_SendEmail()
        {
            var fakeRulesManager = A.Fake<IFileRulesManager>();
            A.CallTo(() => fakeRulesManager.IsValidFileNameLength("abc.ext")).Throws<Exception>();

            Assert.Throws<Exception>(() => fakeRulesManager.IsValidFileNameLength("abc.ext"));
        }


        private static LogAnalyzer MakeAnalyzer(ILoggerService webService, IEmailService emailService)
        {
            return new LogAnalyzer(webService, emailService);
        }
    }
    public class FakeWebService : ILoggerService
    {
        public Exception ToThrow;
        public string LastError { get; set; }

        public void LogError(string message)
        {
            if (ToThrow != null)
            {
                throw ToThrow;
            }

            LastError = message;
        }
    }
}