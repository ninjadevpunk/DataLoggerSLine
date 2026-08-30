using FileSys.Interfaces;
using System.Security.Principal;
using System.Text.Json;

namespace FileSys.Services
{
    public class InstallationRegistry : IInstallationRegistry
    {
        private static readonly string RegistryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "Data Logger", "installations.json");


        public bool RegisterCurrentUser()
        {
            return Modify(registrations =>
            {
                string sid = GetCurrentUserSid();

                if (!registrations.Contains(sid))
                    registrations.Add(sid);
            });
        }

        public bool UnregisterCurrentUser()
        {
            return Modify(registrations =>
            {
                string sid = GetCurrentUserSid();
                registrations.Remove(sid);
            });
        }

        public bool HasInstallations()
        {
            lock (typeof(InstallationRegistry))
            {
                if (!File.Exists(RegistryPath))
                    return false;

                try
                {
                    var registrations = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(RegistryPath));

                    return registrations?.Count > 0;
                }
                catch
                {
                    // Fail safe: if the registry is corrupt, assume installations exist.
                    return true;
                }
            }
        }

        private static bool Modify(Action<List<string>> action)
        {
            lock (typeof(InstallationRegistry))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(RegistryPath)!);

                    List<string> registrations = File.Exists(RegistryPath) ? JsonSerializer.Deserialize<List<string>>(File.ReadAllText(RegistryPath)) ?? new(): new();

                    action(registrations);

                    File.WriteAllText(RegistryPath, JsonSerializer.Serialize(registrations));

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private static string GetCurrentUserSid()
        {
            try
            {
#pragma warning disable CA1416 // Validate platform compatibility
                using WindowsIdentity identity = WindowsIdentity.GetCurrent();

                return identity.User?.Value ?? throw new InvalidOperationException("Unable to determine Windows user SID.");
            }
#pragma warning restore CA1416 // Validate platform compatibility
            catch (global::System.Exception)
            {
                throw;
            }
        }
    }
}
