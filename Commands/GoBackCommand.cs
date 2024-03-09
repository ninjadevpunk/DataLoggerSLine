using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands
{
    public class GoBackCommand : CommandBase
    {
        private readonly NavigationService _navigationService;

        public GoBackCommand(NavigationService navigationService)
        {



            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            }
            catch (Exception)
            {
                //
            }



        }

        public override void Execute(object parameter)
        {
            

            try
            {


                if (_navigationService == null)
                {
                    throw new ArgumentNullException(nameof(_navigationService));
                }

                _navigationService.GoBack();



            }
            catch (Exception)
            {
                //
            }



        }
    }
}
