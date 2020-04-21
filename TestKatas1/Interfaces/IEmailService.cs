namespace TestKatas1.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string address, string subject, string message);
    }
}
