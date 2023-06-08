using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace CSharper.ViewModels
{
    public partial class AdminWindowViewModel : ObservableObject
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

        public AdminWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            ApplicationTitle = "Панель администрирования";

            NavigationItems = new ObservableCollection<INavigationControl>();
            CreatinhNavigation();

            NavigationFooter = new ObservableCollection<INavigationControl>
            {
            };

            _isInitialized = true;
        }

        [RelayCommand]
        private void CreatinhNavigation()
        {
            NavigationItems.Clear();
            NavigationItems.Add(
                new NavigationItem()
                {
                    Content = "Добавить Урок",
                    PageTag = "addLesson",
                    Icon = SymbolRegular.Add12,
                    PageType = typeof(Views.Pages.AdminPages.AddLessonPage)
                }
                );
            NavigationItems.Add(
                new NavigationItem()
                {
                    Content = "Добавить Книгу",
                    PageTag = "addBook",
                    Icon = SymbolRegular.Add16,
                    PageType = typeof(Views.Pages.AdminPages.AddBookPage)
                }
                );
            NavigationItems.Add(
                new NavigationItem()
                {
                    Content = "Добавить Видео",
                    PageTag = "addVideo",
                    Icon = SymbolRegular.Add20,
                    PageType = typeof(Views.Pages.AdminPages.AddVideoPage)
                }
                );
            NavigationItems.Add(
                new NavigationItem()
                {
                    Content = "Добавить Статью",
                    PageTag = "addArticle",
                    Icon = SymbolRegular.Add24,
                    PageType = typeof(Views.Pages.AdminPages.AddArticlePage)
                }
                );
            NavigationItems.Add(
                new NavigationItem()
                {
                    Content = "Добавить Задание",
                    PageTag = "addAssignment",
                    Icon = SymbolRegular.Add28,
                    PageType = typeof(Views.Pages.AdminPages.AddAssignmentPage)
                }
                );
        }

        [RelayCommand]
        private void EditNavigation()
        {
                NavigationItems.Clear();
                NavigationItems.Add(
                    new NavigationItem()
                    {
                        Content = "Редактировать Урок",
                        PageTag = "esitLesson",
                        Icon = SymbolRegular.Edit16,
                        PageType = typeof(Views.Pages.AdminPages.EditLessonPage)
                    }
                    );
                NavigationItems.Add(
                    new NavigationItem()
                    {
                        Content = "Редактировать книгу",
                        PageTag = "esitBook",
                        Icon = SymbolRegular.Edit20,
                        PageType = typeof(Views.Pages.AdminPages.EditBookPage)
                    }
                    );
                NavigationItems.Add(
                    new NavigationItem()
                    {
                        Content = "Редактировать Видео",
                        PageTag = "esitVideo",
                        Icon = SymbolRegular.Edit24,
                        PageType = typeof(Views.Pages.AdminPages.EditVideoPage)
                    }
                    );
                NavigationItems.Add(
                    new NavigationItem()
                    {
                        Content = "Редактировать Статью",
                        PageTag = "esitArticle",
                        Icon = SymbolRegular.Edit28,
                        PageType = typeof(Views.Pages.AdminPages.EditArticlePage)
                    }
                    );
                NavigationItems.Add(
                    new NavigationItem()
                    {
                        Content = "Редактировать Задание",
                        PageTag = "esitAssignment",
                        Icon = SymbolRegular.Edit32,
                        PageType = typeof(Views.Pages.AdminPages.EditAssignmentPage)
                    }
                    );
            }
    }
}
