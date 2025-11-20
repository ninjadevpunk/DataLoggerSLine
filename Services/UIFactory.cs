using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Data_Logger_1._3.Services
{
    public class UIFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public Brush MessageGood { get; set; } = TryParseBrush("GREEN");
        public Brush MessageBad { get; set; } = TryParseBrush("RED");

        public UIFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static Brush TryParseBrush(string value)
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

        public T CreatePage<T>() where T : Page, new()
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public TPage CreatePage<TPage, TViewModel>()
            where TPage : Page, new()
            where TViewModel : class
        {
            var page = new TPage();
            var viewModel = _serviceProvider.GetService<TViewModel>();

            page.DataContext = viewModel;

            return page;
        }
    }
}
