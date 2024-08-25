using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using static Data_Logger_1._3.ViewModels.Dialogs.CreatePostItViewModel;

namespace Data_Logger_1._3.Commands.PostItCommands.ToolCommands
{
    public class HighlighterCommand : CommandBase
    {
        private readonly CreatePostItViewModel _createPostItViewModel;

        public HighlighterCommand(CreatePostItViewModel createPostItViewModel)
        {
            try
            {
                _createPostItViewModel = createPostItViewModel ?? throw new ArgumentNullException(nameof(createPostItViewModel));
            }
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Argument null exception: {nullx.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred: {ex.Message}");
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                var editor = _createPostItViewModel.ActiveEditor;
                if (editor != null)
                {
                    switch (_createPostItViewModel.FieldToEdit)
                    {
                        case PostItField.Error:
                            {
                                editor.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Orange);
                                break;
                            }
                        case PostItField.Solution:
                            {
                                editor.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Lime);
                                break;
                            }
                        case PostItField.Suggestion:
                            {
                                editor.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Pink);
                                break;
                            }
                        case PostItField.Comment:
                            {
                                editor.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
                                break;
                            }
                    }
                }

            }
            catch (Exception)
            {
                //
            }
        }

        private static Brush? TryParseBrush(string value)
        {
            try
            {
                return (Brush)Application.Current.FindResource(value);
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                return Brushes.Transparent;
            }
        }
    }
}
