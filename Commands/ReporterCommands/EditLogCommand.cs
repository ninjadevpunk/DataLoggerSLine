using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Allows the user to edit a log retrieved from the database.
    /// </summary>
    public class EditLogCommand : CommandBase
    {
        public EditLogCommand()
        {
        }

        public override void Execute(object parameter)
        {
            try
            {
                // Your implementation here
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception occurred near EditCommand.Execute(): {e.Message}");
            }
        }
    }

}
