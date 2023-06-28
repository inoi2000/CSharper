using Wpf.Ui.Common.Interfaces;

using System.Windows.Controls;
using System.Windows;
using CSharper.Models;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Linq;

using CSharper.Services;
using System.IO;
using System.Threading;
using System;

namespace CSharper.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class ListAssignmentsPage : INavigableView<ViewModels.ListAssignmentsViewModel>
    {
        public ViewModels.ListAssignmentsViewModel ViewModel
        {
            get;
        }

        public ListAssignmentsPage()
        {
            ViewModel = new ViewModels.ListAssignmentsViewModel();

            InitializeComponent();
        }        

        private async void OpenSelectedAssignment(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DownloadProgresRing.Visibility = Visibility.Visible;
            AssignmentsListBox.Visibility = Visibility.Collapsed;

            await ViewModel.DownloadSelectedAssignment();

            DownloadProgresRing.Visibility = Visibility.Collapsed;
            AssignmentsListBox.Visibility = Visibility.Visible;

            var mainWindow = Application.Current.Windows.OfType<Windows.MainWindow>().First();
            var readerWindow = Application.Current.Windows.OfType<Windows.PdfViewerWindow>().First();

            mainWindow.Visibility = Visibility.Collapsed;

            readerWindow.Open(ViewModel.SelectedAssignment);
            readerWindow.SetPdfReadingService(ViewModel._assignmentService);
            readerWindow.ShowDialog();
            mainWindow.Visibility = Visibility.Visible;

            DownloadProgresRing.Visibility = Visibility.Collapsed;
            AssignmentsListBox.Visibility = Visibility.Visible;
        }

        private async void SelectCurrentSubject(object sender, SelectionChangedEventArgs e)
        {
            await ViewModel.DebounceFilter();
        }


        private async void ChangeFindName(object sender, TextChangedEventArgs e)
        {
            await ViewModel.DebounceFilter();
        }
    }
}