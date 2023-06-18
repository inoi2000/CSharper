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
        private static RelayCommand<Assignment> readingClickCommand=new RelayCommand<Assignment>
            (x =>
            {
                if (x == null) return;
                //Reading r = (x.Reading == Reading.Yes) ? Reading.No : Reading.Yes;
                //x.setReading(r); 
               });
        public static RelayCommand<Assignment> ReadingClickCommand
        {
            get  {   return readingClickCommand;   }
        }

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

            readerWindow.Open(ViewModel.LocalPath);
            readerWindow.ShowDialog();
            mainWindow.Visibility = Visibility.Visible;

            DownloadProgresRing.Visibility = Visibility.Collapsed;
            AssignmentsListBox.Visibility = Visibility.Visible;
        }
    }
}