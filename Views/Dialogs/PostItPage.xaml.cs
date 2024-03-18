using Data_Logger_1._3.ViewModels.Dialogs;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Data_Logger_1._3.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for PostItPage.xaml
    /// </summary>
    public partial class PostItPage : Page
    {
        private readonly CreatePostItViewModel _postItViewModel;

        public PostItPage(CreatePostItViewModel createPostItViewModel)
        {
            InitializeComponent();

            _postItViewModel = createPostItViewModel;
            DataContext = _postItViewModel;


        }

        private void on_Error_Text_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextRange range;
                var doc = this.inputText_ERROR.Document;
                range = new TextRange(doc.ContentStart, doc.ContentEnd);
                _postItViewModel.Display_Error = range.Text;

            }
            catch (Exception)
            {
                //
            }


        }

        private void on_Solution_Text_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextRange range;
                var doc = this.inputText_SOLUTION.Document;
                range = new TextRange(doc.ContentStart, doc.ContentEnd);
                _postItViewModel.Display_Solution = range.Text;
            }
            catch (Exception)
            {
                //
            }
        }

        private void on_Suggestion_Text_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextRange range;
                var doc = this.inputText_SUGGESTION.Document;
                range = new TextRange(doc.ContentStart, doc.ContentEnd);
                _postItViewModel.Display_Suggestion = range.Text;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void on_Comment_Text_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextRange range;
                var doc = this.inputText_COMMENT.Document;
                range = new TextRange(doc.ContentStart, doc.ContentEnd);
                _postItViewModel.Display_Comment = range.Text;
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
