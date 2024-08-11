using Data_Logger_1._3.Models;
using MVVMEssentials.ViewModels;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class NoteLOGViewModel : ViewModelBase
    {
        private readonly NoteItem _noteItem;




        #region Constructors


        public NoteLOGViewModel(NoteItem noteItem)
        {
            _noteItem = noteItem;
        }






        #endregion



        #region Properties



        public string Subject => _noteItem.Subject;

        public string Content => _noteItem.Content;

        public string Date => $"Created {_noteItem.Start.ToString("d/M/yyyy HH:mm:ss")}";







        #endregion















        #region Member Functions






















        #endregion


    }
}
