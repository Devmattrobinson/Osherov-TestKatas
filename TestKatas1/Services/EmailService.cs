using TestKatas1.Interfaces;
using TestKatas1.Models;

namespace TestKatas1.Services
{
    public class EmailService : IEmailService
    {
        public Email Email = new Email();

        public void SendEmail(string to,string subject, string body)
        {
            Email = new Email()
            {
                To = to,
                Subject = subject,
                Body = body
            };
        }
    }
}
