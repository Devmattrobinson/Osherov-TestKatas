namespace TestKatas1.Interfaces {

    public interface IWebService
    {
        void LogError(string message);
        string LastError { get; set; }
    }
}
