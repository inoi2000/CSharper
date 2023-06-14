using System;
using Wpf.Ui.Common.Interfaces;

using System.Windows.Controls;
using System.Windows;
using CSharper.Models;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using CSharper.ViewModels;
using System.Collections.ObjectModel;

namespace CSharper.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class Assignments : INavigableView<ViewModels.AssignmentsViewModel>
    {
        public ViewModels.AssignmentsViewModel ViewModel
        {
            get;
        }

        public Assignments(ViewModels.AssignmentsViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FirstComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SearchComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
