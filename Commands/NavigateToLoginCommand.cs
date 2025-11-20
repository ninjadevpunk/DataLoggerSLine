using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

/// <summary>
/// A command to send the user to the login page.
/// </summary>
public class NavigateToLoginCommand : CommandBase
{
    private readonly NavigationService _navigationService;
    private readonly SignUpViewModel _signUpViewModel;

    public NavigateToLoginCommand(NavigationService navigationService, SignUpViewModel signUpViewModel)
    {
        _navigationService = navigationService;
        _signUpViewModel = signUpViewModel;
    }

    public override void Execute(object parameter)
    {
        try
        {
            _navigationService.NavigateToLogin(false);
            _signUpViewModel.CloseSignUp();
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Exception occurred near NavigateToLoginCommand.Execute(): {e.Message}");
        }
    }
}
