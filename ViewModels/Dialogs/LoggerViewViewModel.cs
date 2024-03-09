using System.Windows;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public abstract class LoggerViewViewModel
    {


        public abstract string LogType { get; }

        public Visibility DisplayPicVisibility { get; set; }

        public string ProjectName { get; set; }

        public string ApplicationName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Output { get; set; }

        public string Type { get; set; }



        ICommand OKButton { get; set; }
    }
}
