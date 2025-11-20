using Data_Logger_1._3.Models;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Microsoft.Extensions.DependencyInjection;

namespace Data_Logger_1._3.Services
{
    public class ViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public CreateChecklistItemViewModel MakeCreateChecklistItemViewModel(CheckListItem checklistItem)
        {
            var createCheckListViewModel = _serviceProvider.GetRequiredService<CreateCheckListViewModel>();

            return new CreateChecklistItemViewModel(createCheckListViewModel, checklistItem);
        }

        public codeCreateViewModel CreateQtCodeCreateViewModel()
        {
            return new codeCreateViewModel(
                _serviceProvider.GetRequiredService<NavigationService>(),
                _serviceProvider.GetRequiredService<CodingQtViewModel>(),
                "Qt",
                _serviceProvider.GetRequiredService<DataService>()
            );
        }

        public codeCreateViewModel CreateCodeCreateViewModel()
        {
            return new codeCreateViewModel(
                _serviceProvider.GetRequiredService<NavigationService>(),
                _serviceProvider.GetRequiredService<CodingViewModel>(), 
                _serviceProvider.GetRequiredService<DataService>()
            );
        }

        public PostItViewModel CreatePostItViewModel(LoggerCreateViewModel loggerCreateViewModel, LOG.CATEGORY category)
        {
            return new PostItViewModel(
                _serviceProvider.GetRequiredService<NavigationService>(),
                _serviceProvider.GetRequiredService<DataService>(),
                loggerCreateViewModel,
                category
                );
        }
    }

}
