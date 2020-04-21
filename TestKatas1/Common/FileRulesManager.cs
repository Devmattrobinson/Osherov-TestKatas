using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestKatas1.Interfaces;

namespace TestKatas1
{
    public class FileRulesManager : IFileRulesManager
    {
        private readonly ILoggerService _logger;
        private readonly IEmailService _emailService;

        public FileRulesManager(ILoggerService service, IEmailService emailService)
        {
            _logger = service;
            _emailService = emailService;
        }

        public bool IsValidExtension(string fileName)
        {
            if (fileName == "")
            {
                throw new Exception("filename has to be provided");
            }
            return fileName.ToLower().EndsWith(".slf");
        }

        public bool IsValidFileNameLength(string fileName)
        {
            if (fileName.Length >= 8) return true;
            try
            {
                _logger.LogError($"Filename too short: {fileName}");
            }
            catch (Exception e)
            {
                _emailService.SendEmail("email1@address.com", "Can't log", e.Message);
            }
            return false;
        }
    }
}
