using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LoggerCommands
{
    public class BrowseCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}
