using Data_Logger_1._3.ViewModels.Reporter;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Data_Logger_1._3.Components
{
    /// <summary>
    /// Interaction logic for SearchInputField.xaml
    /// </summary>
    public partial class SearchInputField : UserControl
    {

        public SearchInputField()
        {
            InitializeComponent();
            UpdatePlaceholderText();
        }

        #region Dependency Properties


        // Placeholder text
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(SearchInputField));



        public string DynamicPlaceholderText
        {
            get { return (string)GetValue(DynamicPlaceholderTextProperty); }
            set { SetValue(DynamicPlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty DynamicPlaceholderTextProperty =
            DependencyProperty.Register("DynamicPlaceholderText", typeof(string), typeof(SearchInputField));




        // User's input
        public string UserComboInput
        {
            get { return (string)GetValue(UserComboInputProperty); }
            set { SetValue(UserComboInputProperty, value); }
        }

        public static readonly DependencyProperty UserComboInputProperty =
            DependencyProperty.Register("UserComboInput", typeof(string), typeof(SearchInputField));

        // Items in the combo box
        public IEnumerable<string> Items
        {
            get { return (IEnumerable<string>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable<string>), typeof(SearchInputField));


        // For enabling and disabling editing the control. Will allow it to be a normal combo box if false.
        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.Register("Enabled", typeof(bool), typeof(SearchInputField), new PropertyMetadata(true));



        // Style for the TextBox in the ComboBox
        public Style UserTextBoxStyle
        {
            get { return (Style)GetValue(UserTextBoxStyleProperty); }
            set { SetValue(UserTextBoxStyleProperty, value); }
        }

        public static readonly DependencyProperty UserTextBoxStyleProperty =
            DependencyProperty.Register("UserTextBoxStyle", typeof(Style), typeof(SearchInputField));


        // TextBlock Style
        public Style UserTextBlockStyle
        {
            get { return (Style)GetValue(UserTextBlockStyleProperty); }
            set { SetValue(UserTextBlockStyleProperty, value); }
        }

        public static readonly DependencyProperty UserTextBlockStyleProperty =
            DependencyProperty.Register("UserTextBlockStyle", typeof(Style), typeof(SearchInputField));



        // List of SearchResults
        public ObservableCollection<SearchResultViewModel> SearchResults
        {
            get { return (ObservableCollection<SearchResultViewModel>)GetValue(SearchResultsProperty); }
            set { SetValue(SearchResultsProperty, value); }
        }

        public static readonly DependencyProperty SearchResultsProperty =
            DependencyProperty.Register("SearchResults", typeof(ObservableCollection<SearchResultViewModel>), typeof(SearchInputField));



        // Search Bar Opens and Closes Popup
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(SearchInputField), new PropertyMetadata(false));



        // Enter Button Must Execute Search

        public ICommand EnterInternalCommand
        {
            get { return (ICommand)GetValue(EnterInternalCommandProperty); }
            set { SetValue(EnterInternalCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnterInternalCommandProperty =
            DependencyProperty.Register("EnterInternalCommand", typeof(ICommand), typeof(SearchInputField));





        #endregion







        #region Member Functions


        private void on_EditableTextBox_lostfocus(object sender, RoutedEventArgs e)
        {
            UpdatePlaceholderText();
            IsDropDownOpen = false;
        }

        public void UpdatePlaceholderText()
        {
            DynamicPlaceholderText = UserComboInput is not null && UserComboInput.Equals(string.Empty) ? PlaceholderText : string.Empty;
        }



        #endregion







        #region Event Handlers



        private void on_EditableTextBox_changed(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholderText();
            IsDropDownOpen = false;

            var ev = new RoutedEventArgs(TextChangedEvent);
            RaiseEvent(ev);
        }






        #endregion






        #region Routed Events


        public event RoutedEventHandler TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }

        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent(
            "TextChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SearchInputField));








        #endregion
    }
}
