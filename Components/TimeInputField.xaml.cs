using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Components
{
    /// <summary>
    /// Interaction logic for TimeInputField.xaml
    /// </summary>
    public partial class TimeInputField : UserControl
    {
        public TimeInputField()
        {
            InitializeComponent();
            setTime();

        }


        #region Dependency Properties




        public string Hours
        {
            get { return (string)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }

        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register("Hours", typeof(string), typeof(TimeInputField), 
                
                
                new PropertyMetadata("00")
                );



        public string Minutes
        {
            get { return (string)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }

        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes", typeof(string), typeof(TimeInputField), 
                
                
                new PropertyMetadata("00")
                );



        public string Seconds
        {
            get { return (string)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        public static readonly DependencyProperty SecondsProperty =
            DependencyProperty.Register("Seconds", typeof(string), typeof(TimeInputField), 
                
                
                new PropertyMetadata("00")
                );



        public string Milliseconds
        {
            get { return (string)GetValue(MillisecondsProperty); }
            set { SetValue(MillisecondsProperty, value); }
        }

        public static readonly DependencyProperty MillisecondsProperty =
            DependencyProperty.Register("Milliseconds", typeof(string), typeof(TimeInputField), 
                
                
                new PropertyMetadata("000")
                );

        public DateTime Time
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(DateTime), typeof(TimeInputField), 
                
                
                new PropertyMetadata(DateTime.Parse(DateTime.Now.ToString("HH:mm:ss.fff")))
                );




        #endregion





        #region Member Functions


        public void setTime()
        {
            Time = DateTime.Now;

            Hours = DateTime.Now.Hour.ToString();
            Minutes = DateTime.Now.Minute.ToString();
            Seconds = DateTime.Now.Second.ToString();
            Milliseconds = DateTime.Now.Millisecond.ToString();
        }





        #endregion





        #region Event Handlers

        private void updateTime(object sender, RoutedEventArgs e)
        {
            if (this.spinner_MILLISECONDS is null)
                return;

            try
            {
                int milliseconds_count = this.spinner_MILLISECONDS.Digits - Milliseconds.Length;

                if (milliseconds_count != 0 && milliseconds_count > 0)
                {
                    const string zero = "0";


                    for (int i = 0; i < milliseconds_count; i++)
                        Milliseconds = zero + Milliseconds;
                }

                this.Time = DateTime.Parse(
                Hours + ":" +
                Minutes + ":" +
                Seconds + "." +
                Milliseconds);
            }
            catch (Exception)
            {
                setTime();
            }

            var ev = new RoutedEventArgs(TimeChangedEvent);
            RaiseEvent(ev);
        }




        #endregion






        #region Routed Events






        public event RoutedEventHandler TimeChanged
        {
            add { AddHandler(TimeChangedEvent, value); }
            remove { RemoveHandler(TimeChangedEvent, value); }
        }

        public static readonly RoutedEvent TimeChangedEvent = EventManager.RegisterRoutedEvent(
            "TimeChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(TimeInputField));




        #endregion

    }
}
