using CSharper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Common.Interfaces;

namespace CSharper.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для VideosPage.xaml
    /// </summary>
    public partial class VideosPage : INavigableView<ViewModels.VideosViewModel>
    {
        public ViewModels.VideosViewModel ViewModel { get; }

        public VideosPage(ViewModels.VideosViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string destinationUrl = (videosListbox.SelectedItem as Video).Url.ToString();
            var startInfo = new System.Diagnostics.ProcessStartInfo(destinationUrl)
            {
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(startInfo);
        }

        private async void subjectsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ViewModel.UpdateVideosList();
        }
    }
}
