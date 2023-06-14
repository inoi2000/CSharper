using CommunityToolkit.Mvvm.Input;
using CSharper.Models;
using CSharper.Services;
using CSharper.ViewModels;
using CSharper.Views.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Wpf.Ui.Common.Interfaces;

namespace CSharper.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class AutorizationPage : INavigableView<ViewModels.AutorizationViewModel>
    {
 
        public ViewModels.AutorizationViewModel ViewModel
        {
            get;
        }

        public AutorizationPage(ViewModels.AutorizationViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

     }
}