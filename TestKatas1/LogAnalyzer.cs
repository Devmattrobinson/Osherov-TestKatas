using System;
using System.ComponentModel.DataAnnotations;
using TestKatas1.Interfaces;
using TestKatas1.Services;

namespace TestKatas1
{
    public class LogAnalyzer
    {
        private readonly IFileExtensionManager _manager;
        private readonly IWebService _webService;
        private readonly IEmailService _emailService;

        public LogAnalyzer(IFileExtensionManager mgr, IWebService service, IEmailService emailService)
        {
            _manager = mgr;
            _webService = service;
            _emailService = emailService;
        }
        public bool IsValidLogFileName(string fileName)
        {
            if (fileName.Length < 8)
            {
                try
                {
                    _webService.LogError($"Filename too short: {fileName}");
                }
                catch (Exception e)
                {
                    _emailService.SendEmail("email1@address.com", "Can't log", e.Message);
                }
            }

            return _manager.IsValid(fileName);
        }
    }
}
