using NUnit.Framework;
using System;
using TestKatas1;
using TestKatas1.Interfaces;
using FakeItEasy;
using TestKatas1.Models;
using TestKatas1.Services;

namespace LogAnalyzerUnitTests
{
    [TestFixture]
    public class LogAnalyzerUnitTests
    {
        private FileExtensionManager _manager;
        private IWebService _webService;
        private IEmailService _emailService;

        [SetUp]
        public void Setup()
        {
            _manager = A.Fake<FileExtensionManager>();
            _webService = A.Fake<FakeWebService>();
            _emailService = A.Fake<EmailService>();

        }

        [TestCase("filewithbadextension.foo", false)]
        [TestCase("filewithgoodextension.slf", true)]
        [TestCase("filewithgoodextension.SLF", true)]
        public void IsValidFileName_BadExtension_ReturnsFalse(string filename, bool expected)
        {

            LogAnalyzer log = MakeAnalyzer(_manager, _webService, _emailService);
            bool result = log.IsValidLogFileName(filename);
            
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void IsValidFileName_EmptyFileName_Throws()
        {
            var _analyzer = MakeAnalyzer(_manager, _webService, _emailService);
            var ex = Assert.Catch<Exception>(() => _analyzer.IsValidLogFileName(""));

            StringAssert.Contains("filename has to be provided", ex.Message);
        }

        [Test]
        public void Analyze_TooShortFileName_CallsWebService()
        {
            LogAnalyzer log = new LogAnalyzer(_manager, _webService, _emailService);
            string tooShortFileName = "abc.ext";
            log.IsValidLogFileName(tooShortFileName);
            StringAssert.Contains("Filename too short: abc.ext",
                _webService.LastError);
        }

        [Test]
        public void Analyze_Webservice_SendEmail()
        {
            var expectedEmail = new Email()
            {
                To = "email1@address.com",
                Subject = "Can't Log'",
                Body = "fake exception"
            };
            FakeWebService stubService = new FakeWebService();
            stubService.ToThrow = new Exception("fake exception");
            var emailService = new EmailService();
            var analyzer = MakeAnalyzer(_manager, stubService, emailService);
            string tooShortFileName = "abc.ext";
            
            analyzer.IsValidLogFileName(tooShortFileName);

            Assert.AreEqual(expectedEmail, emailService.Email);
            //StringAssert.Contains("email1@address.com", mockEmailService.email.To);
            //StringAssert.Contains("fake exception", mockEmailService.email.Body);
            //StringAssert.Contains("Can't log", mockEmailService.email.Subject);
        }

        private static LogAnalyzer MakeAnalyzer(IFileExtensionManager mgr, IWebService service, IEmailService emailService)
        {
            return new LogAnalyzer(mgr, service, emailService);
        }
    }
    public class FakeWebService : IWebService
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