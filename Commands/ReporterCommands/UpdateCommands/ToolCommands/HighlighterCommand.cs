using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using static Data_Logger_1._3.ViewModels.Reporter.Updater.CreateReporterPostItViewModel;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands.ToolCommands
{
    public class HighlighterCommand : CommandBase
    {
        private readonly CreateReporterPostItViewModel _postItViewModel;

        public HighlighterCommand(CreateReporterPostItViewModel createReporterPostItViewModel)
        {
            try
            {
                _postItViewModel = createReporterPostItViewModel ?? throw new ArgumentNullException(nameof(createReporterPostItViewModel));
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
                var editor = _postItViewModel.ActiveEditor;
                if (editor != null)
                {
                    switch (_postItViewModel.FieldToEdit)
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
