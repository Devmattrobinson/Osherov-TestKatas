namespace TestKatas1.Interfaces
{
    public interface IFileRulesManager
    {
        bool IsValidExtension(string fileName);
        bool IsValidFileNameLength(string fileName);
    }
}
