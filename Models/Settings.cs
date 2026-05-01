using Data_Logger_1._3.Models.App_Models;

namespace Data_Logger_1._3.Models
{
    public enum Theme
    {
        Default,
        Light,
        Grey
    }

    public class Settings
    {
        public enum Theme
        {
            Default,
            Light,
            Grey
        }

        public Theme AppTheme { get; set; } = Theme.Default;

        public UserSettings User { get; set; } = new UserSettings();

        public void SelectedTheme(string theme)
        {
            switch (theme)
            {
                case "Light":
                    {
                        AppTheme = Theme.Light;
                        break;
                    }
                case "Grey":
                    {
                        AppTheme = Theme.Grey;
                        break;
                    }
                default:
                    {
                        AppTheme = Theme.Default;
                        return;
                    }
            }
        }

    }
}
