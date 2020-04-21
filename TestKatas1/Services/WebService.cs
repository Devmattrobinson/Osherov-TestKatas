using TestKatas1.Interfaces;

namespace TestKatas1.Services
{
    public class FakeWebService : IWebService
    {
        public string LastError { get; set; }

        public void LogError(string message)
        {
            LastError = message;
        }
    }
}
