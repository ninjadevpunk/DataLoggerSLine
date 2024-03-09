using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Windows.Controls;

namespace Data_Logger_1._3.Views.LogPages
{
    /// <summary>
    /// Interaction logic for CreateNotePage.xaml
    /// </summary>
    public partial class CreateNotePage : Page
    {
        public string Created { get; set; } = date();


        public CreateNotePage()
        {
            InitializeComponent();

            this.text_DATE.Text = "Created " + Created + ". Last modified " + Created;
        }

        private void on_NOTEPAD_modified(object sender, TextChangedEventArgs e)
        {
            this.text_DATE.Text = "Created " + Created + ". Last modified " + date();
        }

        public static string date() 
        { 
            return DateTime.Now.ToString("d MMMM yyyy HH:mm");
        }
    }
}
