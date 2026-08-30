namespace Core.Interfaces
{
    public interface IElevationService
    {
        bool IsAdministrator();
        bool ExecuteElevated(string fileName, string arguments);
    }
}
