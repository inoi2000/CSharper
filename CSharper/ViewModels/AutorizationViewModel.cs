using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharper.Models;
using CSharper.Services;
using CSharper.Services.AuthenticationServices;
using CSharper.Views.Windows;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Wpf.Ui.Common.Interfaces;

namespace CSharper.ViewModels
{
    public partial class AutorizationViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private AuthorizationService _authorizationService;
        private RegistrationService _registrationService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegistrationCommand))]
        [NotifyCanExecuteChangedFor(nameof(AuthorizationCommand))]
        private string _login = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegistrationCommand))]
        [NotifyCanExecuteChangedFor(nameof(AuthorizationCommand))]
        private string _password = string.Empty;

        private RelayCommand noAutorizationCommand = new RelayCommand
          (() =>
          {
              //UserService userService = new UserService();
              //int id = 1;

              var mainWindow = Application.Current.Windows.OfType<Views.Windows.MainWindow>().First();
              mainWindow.Navigate(typeof(Views.Pages.HomePage));
          });

        public RelayCommand NoAutorizationCommand
        {
            get { return noAutorizationCommand; }
        }

        [RelayCommand(CanExecute = nameof(CanAutorizatUser))]
        private async Task Authorization()
        {
            _authorizationService = new AuthorizationService();
            if (await _authorizationService.Login(Login, Password)) 
            {
                Application.Current.Windows.OfType<Views.Windows.MainWindow>().First()
                    .Navigate(typeof(Views.Pages.HomePage));
            }
        }

        [RelayCommand(CanExecute = nameof(CanAutorizatUser))]
        private async Task Registration()
        {
            _registrationService = new RegistrationService();
            if (await _registrationService.RegistratUser(Login, Password))
            {
                Application.Current.Windows.OfType<Views.Windows.MainWindow>().First()
                    .Navigate(typeof(Views.Pages.HomePage));
            }
        }
        private bool CanAutorizatUser()
        {
            return !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {            
            _isInitialized = true;
        }
        
    }
}
