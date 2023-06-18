﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using CSharper.Models;
using System.Collections.Generic;
using Apitron.PDF.Rasterizer.Navigation;
using Apitron.PDF.Rasterizer;
using System.ComponentModel;

namespace CSharper.ViewModels
{
    using System.Collections.ObjectModel;
    using Wpf.Ui.Controls;
    using Wpf.Ui.Controls.Interfaces;

    public partial class PdfViewerWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();


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
            ApplicationTitle = "WPF UI - CSharper";
            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home"
                }


            };

            _isInitialized = true;
        }
        #region Fields

        public event PropertyChangedEventHandler PropertyChanged;

        private Document document;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the document.
        /// </summary>
        /// <value> The document. </value>
        public Document Document
        {
            get
            {
                return document;
            }
            set
            {
                if (document == value)
                {
                    return;
                }

                if (document != null)
                {
                    document.Navigator.Navigated -= OnNavigated;
                }

                document = value;
                document.Navigator.Navigated += OnNavigated;
                OnPropertyChanged("Document");
                OnPropertyChanged("Bookmark");
                OnPropertyChanged("Page");
                OnPropertyChanged("Pages");
                OnPropertyChanged("Title");
                OnPropertyChanged("Links");
            }
        }

        /// <summary>
        ///   Gets the page.
        /// </summary>
        public Page Page
        {
            get
            {
                if (document == null)
                {
                    return null;
                }
                return document.Navigator.CurrentPage;
            }
            set
            {
                document.Navigator.GoToPage(value);
            }
        }


        /// <summary>
        ///   Gets the pages.
        /// </summary>
        public PageCollection Pages
        {
            get
            {
                if (document == null)
                {
                    return null;
                }
                return document.Pages;
            }
        }

        /// <summary>
        ///   Gets the bookmark.
        /// </summary>
        public Bookmark Bookmark
        {
            get
            {
                if (document == null)
                {
                    return null;
                }
                return document.Bookmarks;
            }
        }

        /// <summary>
        ///   Gets the document's title.
        /// </summary>
        public string Title
        {
            get
            {
                if (document == null)
                {
                    return null;
                }
                if (document.DocumentInfo.Title == string.Empty)
                {
                    return "WPF Viewer";
                }
                return document.DocumentInfo.Title;
            }
        }

        #endregion

        #region Private Members

        /// <summary>
        ///   Navigators the on page changed.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="eventArgs"> The <see cref="NavigatedEventArgs" /> instance containing the event data. </param>
        private void OnNavigated(object sender, NavigatedEventArgs eventArgs)
        {
            OnPropertyChanged("Page");
            OnPropertyChanged("Links");
        }

        /// <summary>
        ///   Called when [property changed].
        /// </summary>
        /// <param name="propertyName"> Name of the property. </param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
