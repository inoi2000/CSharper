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

namespace CSharper.Views.Pages.AdminPages
{
    public partial class AddArticlePage : INavigableView<ViewModels.AdminViewModels.AddArticleViewModel>
    {
        public ViewModels.AdminViewModels.AddArticleViewModel ViewModel
        {
            get;
        }

        public AddArticlePage(ViewModels.AdminViewModels.AddArticleViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
