using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Deletes a log in the database from the Reporter Desk.
    /// </summary>
    public class DeleteLogCommand : CommandBase
    {
        public DeleteLogCommand()
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
                Debug.WriteLine($"Exception occurred near DeleteLogCommand.Execute(): {e.Message}");
            }
        }
    }

}
