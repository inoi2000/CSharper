﻿using System;
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
    public partial class AddSubjectPage : INavigableView<ViewModels.AdminViewModels.AddSubjectViewModel>
    {
        public ViewModels.AdminViewModels.AddSubjectViewModel ViewModel
        {
            get;
        }

        public AddSubjectPage(ViewModels.AdminViewModels.AddSubjectViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
