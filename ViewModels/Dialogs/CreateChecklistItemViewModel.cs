using Data_Logger_1._3.Models;
using MVVMEssentials.ViewModels;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class CreateChecklistItemViewModel : ViewModelBase
    {
        private readonly CheckListItem _checkListItem;


        #region Constructors 



        public CreateChecklistItemViewModel()
        {
            //
        }

        public CreateChecklistItemViewModel(CheckListItem checkListItem)
        {
            _checkListItem = checkListItem;
        }




        #endregion




        #region Properties



        public bool IsDone => _checkListItem.IsChecked;

        public string Item => _checkListItem.Item;





        #endregion




        #region Member Functions























        #endregion

    }
}
