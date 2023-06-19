﻿using CommunityToolkit.Mvvm.ComponentModel;
using CSharper.Models;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace CSharper.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            ApplicationTitle = "WPF UI - CSharper";

            NavigationItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Начальная страница",
                    PageTag = "home",
                    Icon = SymbolRegular.Home24,
                    PageType = typeof(Views.Pages.HomePage)
                },
                new NavigationItem()
                {
                    Content = "Литература",
                    PageTag = "books",
                    Icon = SymbolRegular.Book24,
                    PageType = typeof(Views.Pages.ListBooksPage)
                },
                new NavigationItem()
                {
                    Content = "Уроки",
                    PageTag = "lessons",
                    Icon = SymbolRegular.BookTheta24,
                    PageType = typeof(Views.Pages.ListLessonsPage)
                },
                new NavigationItem()
                {
                    Content = "Видео",
                    PageTag = "videos",
                    Icon = SymbolRegular.Video24,
                    PageType = typeof(Views.Pages.VideosPage)
                },
                new NavigationItem()
                {
                    Content = "Статьи",
                    PageTag = "articles",
                    Icon = SymbolRegular.Globe24,
                    PageType = typeof(Views.Pages.ArticlesPage)
                },
                new NavigationItem()
                {
                    Content = "Практические задания",
                    PageTag = "assignments",
                    Icon = SymbolRegular.BookCoins20,
                    PageType = typeof(Views.Pages.ListAssignmentsPage)
                }
            };

            NavigationFooter = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Авторизация",
                    PageTag = "autorization",
                    Icon = SymbolRegular.HomeAdd24,
                    PageType = typeof(Views.Pages.AutorizationPage)
                },
                new NavigationItem()
                {
                    Content = "Settings",
                    PageTag = "settings",
                    Icon = SymbolRegular.Settings24,
                    PageType = typeof(Views.Pages.SettingsPage)
                }
            };

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home"
                }


            };

            _isInitialized = true;
        }
    }
}
