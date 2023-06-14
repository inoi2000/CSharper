using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharper.Models;
using CSharper.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;

namespace CSharper.ViewModels
{
    public partial class AutorizationViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private static UserService userService= new UserService();
        private RelayCommand autorizationCommand = new RelayCommand
        ( async () =>
        {
            int id = 1;
         //   User user =await userService.GetUserAsync(System.Guid.Parse(id.ToString()));
          //  AppConfig.User = user;
        
            var mainWindow = Application.Current.Windows.OfType<Views.Windows.MainWindow>().First();
            mainWindow.Navigate(typeof(Views.Pages.HomePage));
        });

        public RelayCommand AutorizationCommand
        {
            get { return autorizationCommand; }
        }

        private RelayCommand noAutorizationCommand = new RelayCommand
          (() =>
          {
              UserService userService = new UserService();
              int id = 1;

              var mainWindow = Application.Current.Windows.OfType<Views.Windows.MainWindow>().First();
              mainWindow.Navigate(typeof(Views.Pages.HomePage));
          });

        public RelayCommand NoAutorizationCommand
        {
            get { return noAutorizationCommand; }
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
