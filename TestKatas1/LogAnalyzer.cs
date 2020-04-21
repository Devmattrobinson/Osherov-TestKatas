
using TestKatas1.Interfaces;

namespace TestKatas1
{
    public class LogAnalyzer
    {
        private readonly FileRulesManager _fileManager;

        public LogAnalyzer(ILoggerService webService, IEmailService emailService)
        {
           _fileManager = new FileRulesManager(webService, emailService);
        }

        public void ProcessFile(string fileName)
        {
            _fileManager.IsValidExtension(fileName);
            _fileManager.IsValidFileNameLength(fileName);
        }
    }
}
