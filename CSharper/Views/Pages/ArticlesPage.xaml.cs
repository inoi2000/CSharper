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
    /// Логика взаимодействия для ArticlesPage.xaml
    /// </summary>
    public partial class ArticlesPage : INavigableView<ViewModels.ArticlesViewModel>
    {
        public ViewModels.ArticlesViewModel ViewModel
        {
            get;
        }

        public ArticlesPage(ViewModels.ArticlesViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            //ViewModel.SelectedSubject = subjectsComboBox.SelectedItem as Subject;
        }


        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string destinationUrl = (articlesListbox.SelectedItem as Article).Url.ToString();
            var startInfo = new System.Diagnostics.ProcessStartInfo(destinationUrl)
            {
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(startInfo);
        }

        private void subjectsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.UpdateArticlesList();
        }
    }
}