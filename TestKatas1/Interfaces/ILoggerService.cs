namespace TestKatas1.Interfaces {

    public interface ILoggerService
    {
        void LogError(string message);
        string LastError { get; set; }
    }
}
