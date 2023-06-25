using Wpf.Ui.Common.Interfaces;

using System.Windows.Controls;
using System.Windows;
using CSharper.Models;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Linq;


namespace CSharper.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class ListLessonsPage : INavigableView<ViewModels.ListLessonsViewModel>
    {
        private static RelayCommand<Lesson> readingClickCommand = new RelayCommand<Lesson>
                  (x =>
                  {
                      if (x == null) return;
                      //Reading r = (x.Reading == Reading.Yes) ? Reading.No : Reading.Yes;
                      //x.setReading(r); 
                  });
        public static RelayCommand<Lesson> ReadingClickCommand
        {
            get { return readingClickCommand; }
        }
        public ViewModels.ListLessonsViewModel ViewModel
        {
            get;
        }

        public ListLessonsPage()
        {
             ViewModel = new ViewModels.ListLessonsViewModel();
             
             InitializeComponent();
        }

        private void SelectListLesson(object sender, System.Windows.RoutedEventArgs e)
        {
             ((sender as ComboBox).SelectedItem as RelayCommand).Execute(null);
        }
        

        private async void OpenSelectedLesson(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            DownloadProgresRing.Visibility = Visibility.Visible;
            LessonsListBox.Visibility = Visibility.Collapsed;

            await ViewModel.DownloadSelectedLesson();

            DownloadProgresRing.Visibility = Visibility.Collapsed;
            LessonsListBox.Visibility = Visibility.Visible;

            var mainWindow = Application.Current.Windows.OfType<Windows.MainWindow>().First();
            var readerWindow = Application.Current.Windows.OfType<Windows.PdfViewerWindow>().First();

            mainWindow.Visibility = Visibility.Collapsed;

            readerWindow.Open(ViewModel.SelectedLesson);
            readerWindow.SetPdfReadingService(ViewModel._lessonService);
            readerWindow.ShowDialog();
            mainWindow.Visibility = Visibility.Visible;

            DownloadProgresRing.Visibility = Visibility.Collapsed;
            LessonsListBox.Visibility = Visibility.Visible;

        }

        private void SelectListLesson(object sender, SelectionChangedEventArgs e)
        {
            //ViewModel.SelectCommands[ ((sender as ComboBox).SelectedItem as string)].Execute(null);

        }

        private async void SelectCurrentSubject(object sender, SelectionChangedEventArgs e)
        {
            await ViewModel.GetLessonsOnFilter();
        }

        private async void ChangeFindName(object sender, TextChangedEventArgs e)
        {
            await ViewModel.GetLessonsOnFilter();
        }
    }
}