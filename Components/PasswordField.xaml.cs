using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Components
{
    /// <summary>
    /// Interaction logic for PasswordField.xaml
    /// </summary>
    public partial class PasswordField : UserControl
    {
        public bool Status { get; set; } = true;
        public string Temp { get; set; } = "";

        public PasswordField()
        {
            InitializeComponent();
            DataContext = this;
        }


        #region Dependency Properties


        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(PasswordField));


        public string UserInput
        {
            get { return (string)GetValue(UserInputProperty); }
            set { SetValue(UserInputProperty, value); }
        }

        public static readonly DependencyProperty UserInputProperty =
            DependencyProperty.Register("UserInput", typeof(string), typeof(PasswordField));



        // Text Alignment
        public string Aligned
        {
            get { return (string)GetValue(AlignedProperty); }
            set { SetValue(AlignedProperty, value); }
        }

        public static readonly DependencyProperty AlignedProperty =
            DependencyProperty.Register("Aligned", typeof(string), typeof(PasswordField));






        #endregion


        #region Event Handlers

        private void on_Password_changed(object sender, RoutedEventArgs e)
        {
            if (Status)
            {
                Temp = this.text_Placeholder_text.Text;
                this.text_Placeholder_text.Text = "";
                Status = false;

            }
            
            UserInput = this.inputText_password.Password;
            
            var ev = new RoutedEventArgs(PasswordChangedEvent);
            RaiseEvent(ev);
        }


        private void on_Password_lostFocus(object sender, RoutedEventArgs e)
        {
            showPlaceholderText();
        }


        public void showPlaceholderText()
        {
            if (this.inputText_password.Password == null)
            {
                return;
            }
            else if (this.inputText_password.Password == "" && this.text_Placeholder_text.Text == "")
            {
                this.text_Placeholder_text.Text = Temp;
                Status = true;
            }
        }



        #endregion







        #region Routed Events


        public event RoutedEventHandler PasswordChanged
        {
            add { AddHandler(PasswordChangedEvent, value); }
            remove { RemoveHandler(PasswordChangedEvent, value); }
        }

        public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent(
            "PasswordChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PasswordField));








        #endregion

    }
}
