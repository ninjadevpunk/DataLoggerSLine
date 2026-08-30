namespace FileSys.Interfaces
{
    public interface IInstallationRegistry
    {
        bool RegisterCurrentUser();
        bool UnregisterCurrentUser();
        bool HasInstallations();
    }
}
