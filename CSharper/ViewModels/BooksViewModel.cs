using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using CSharper.Models;
using System.Collections.Generic;

using System.Windows;

namespace CSharper.ViewModels
{
    public partial class BooksViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        //private ActionCommand _readingClickCommand = new ActionCommand(x => { MessageBox.Show(""); }, null, "Открыть pdf-файл");

        //public ActionCommand ReadingClickCommand 
        //    {
        //        get
        //        {
                    
        //            return _readingClickCommand;
        //        }
        //   }
    
                //<MenuItem Command = "{x:Static ApplicationCommands.Cut}" CommandParameter="Cut it!"/>
                //<MenuItem Command = "{x:Static ApplicationCommands.Copy}" CommandParameter="Copy it!"/>
                //<MenuItem Command = "{x:Static ApplicationCommands.Paste}" CommandParameter="Paste it!"/>
           
        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

 
        private void InitializeViewModel()
        {            
;           _isInitialized = true;
        }

     }
}
