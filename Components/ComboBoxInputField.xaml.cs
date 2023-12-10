using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Components
{
    /// <summary>
    /// Interaction logic for ComboBoxInputField.xaml
    /// </summary>
    public partial class ComboBoxInputField : UserControl
    {
        public bool Status { get; set; } = true;
        public string? Temp { get; set; } = "";

        public ComboBoxInputField()
        {
            InitializeComponent();
        }





        #region Dependency Properties


        // Placeholder text
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(ComboBoxInputField));


        // User's input
        public string UserComboInput
        {
            get { return (string)GetValue(UserComboInputProperty); }
            set { SetValue(UserComboInputProperty, value); }
        }

        public static readonly DependencyProperty UserComboInputProperty =
            DependencyProperty.Register("UserComboInput", typeof(string), typeof(ComboBoxInputField));

        // Items in the combo box
        public IEnumerable<string> Items
        {
            get { return (IEnumerable<string>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable<string>), typeof(ComboBoxInputField));


        // For enabling and disabling editing the control. Will allow it to be a normal combo box if false.
        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.Register("Enabled", typeof(bool), typeof(ComboBoxInputField), new PropertyMetadata(true));



        // Style for the TextBox in the ComboBox
        public Style UserTextBoxStyle
        {
            get { return (Style)GetValue(UserTextBoxStyleProperty); }
            set { SetValue(UserTextBoxStyleProperty, value); }
        }

        public static readonly DependencyProperty UserTextBoxStyleProperty =
            DependencyProperty.Register("UserTextBoxStyle", typeof(Style), typeof(ComboBoxInputField) );


        // TextBlock Style
        public Style UserTextBlockStyle
        {
            get { return (Style)GetValue(UserTextBlockStyleProperty); }
            set { SetValue(UserTextBlockStyleProperty, value); }
        }

        public static readonly DependencyProperty UserTextBlockStyleProperty =
            DependencyProperty.Register("UserTextBlockStyle", typeof(Style), typeof(ComboBoxInputField));





        #endregion







        #region Member Functions


        private void on_EditableTextBox_lostfocus(object sender, RoutedEventArgs e)
        {
            showPlaceholderText();
        }

        public void showPlaceholderText()
        {
            if (this.comboBox_TEXTBOX.Text == null)
            {
                return;
            }
            else if (this.comboBox_TEXTBOX.Text == "" && this.text_Placeholdertext.Text == "")
            {
                this.text_Placeholdertext.Text = Temp;
                Status = true;
            }
        }

        public void showPlaceholderText(bool show, bool assert)
        {
            if (show)
            {
                this.text_Placeholdertext.Text = Temp;
                Status = true;
            }
            else if (show && assert || this.comboBox_TEXTBOX.Text == "")
            {
                this.text_Placeholdertext.Text = PlaceholderText;
                Status = true;
            }
        }








        #endregion







        #region Event Handlers


        private void on_EditableTextBox_changed(object sender, TextChangedEventArgs e)
        {
            if(Status)
            {
                Temp = this.text_Placeholdertext.Text;
                this.text_Placeholdertext.Text = "";
                Status = false;
            }

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
            typeof(ComboBoxInputField));








        #endregion

    }
}
