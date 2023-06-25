using Apitron.PDF.Rasterizer;
using Apitron.PDF.Rasterizer.Configuration;
using Apitron.PDF.Rasterizer.Navigation;
using CSharper.Models;
using CSharper.Services;
using CSharper.ViewModels;

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;
using Page = Apitron.PDF.Rasterizer.Page;


namespace CSharper.Views.Windows
{
    using Button = System.Windows.Controls.Button;
   
    using Rectangle = Apitron.PDF.Rasterizer.Rectangle;
    /// <summary>
    /// Логика взаимодействия для PdfViewerPage.xaml
    /// </summary>
    public partial class PdfViewerWindow : INavigationWindow,INavigableView<PdfViewerWindowViewModel>
    {

        public PdfViewerWindow(ViewModels.PdfViewerWindowViewModel viewModel, IPageService pageService, INavigationService navigationService)
        {
            ViewModel = viewModel;
            DataContext = this;
            this.document = new PdfViewerWindowViewModel();
            this.DataContext = this.document;
            this.document.PropertyChanged += DocumentOnPropertyChanged;

            InitializeComponent();
        
            navigationService.SetNavigationControl(null);
        }


        public PdfViewerWindowViewModel ViewModel { get; }
        
        private IPdfReading _pdfReading { get; set; }
        private IPdfReadingService _pdfReadingService { get; set; }

        public void Open(IPdfReading pdfReading)
        {
            _pdfReading = pdfReading;
            try
            {
                Document document = new Document(new FileStream(pdfReading.LocalLink!, FileMode.Open, FileAccess.Read));
                (this.document).Document = document;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }


        public void SetPdfReadingService(IPdfReadingService pdfReadingService)
        {
            _pdfReadingService = pdfReadingService;
        }

        #region Fields

        private delegate void SetImageSourceDelegate(byte[] source, IList<Link> links, int width, int height);

        public PdfViewerWindowViewModel document = null;

        private Task task;

        private int GlobalScale = 2;

        private Rectangle destinationRectangle;

        #endregion

        #region Private Members

        private void UpdatePageView()
        {
            Page currentPage = this.document.Page;
            int desiredWidth = (int)currentPage.Width * GlobalScale;
            int desiredHeight = (int)currentPage.Height * GlobalScale;
            byte[] image = currentPage.RenderAsBytes(desiredWidth, desiredHeight, new RenderingSettings());
            IList<Link> links = currentPage.Links;

            SetImageSourceDelegate del = this.SetImageSource;
            this.Dispatcher.Invoke(del, image, links, desiredWidth, desiredHeight);
        }

        private void SetImageSource(byte[] image, IList<Link> links, int width, int height)
        {
            BitmapSource source = BitmapSource.Create(width, height, 72, 72, PixelFormats.Bgr32, null, image, 4 * width);
            this.PageImage.Source = source;
            this.PageImage.Width = width;
            this.PageImage.Height = height;

            this.PageCanvas.Width = width;
            this.PageCanvas.Height = height;

            for (int i = 1; i < this.PageCanvas.Children.Count; i++)
            {
                Button rectangle = (Button)this.PageCanvas.Children[i];
                rectangle.Click -= this.OnLinkClick;
            }

            this.PageCanvas.Children.RemoveRange(1, this.PageCanvas.Children.Count);

            foreach (Link link in links)
            {
                Apitron.PDF.Rasterizer.Rectangle location = link.GetLocationRectangle(width, height, null);
                Button rectangle = new Button();
                rectangle.Opacity = 0;
                rectangle.Cursor = Cursors.Hand;
                rectangle.Width = location.Right - location.Left;
                rectangle.Height = location.Top - location.Bottom;
                Canvas.SetLeft(rectangle, location.Left);
                Canvas.SetBottom(rectangle, location.Bottom);
                rectangle.Click += this.OnLinkClick;
                rectangle.DataContext = link;
                this.PageCanvas.Children.Add(rectangle);
            }

            this.UpdateImageZoom();
            this.UpdateViewLocation(this.destinationRectangle);
        }

        private void UpdateImageZoom()
        {
            Slider slider = this.ZoomSlider;
            Canvas imageContainer = this.PageCanvas;
            if (imageContainer != null && slider != null && this.document != null)
            {
                double scale = slider.Value;
                imageContainer.LayoutTransform = new ScaleTransform(scale, scale);
            }
        }

        /// <summary>
        /// Updates the view location.
        /// </summary>
        /// <param name="destinationInfo">The destination info.</param>
        private void UpdateViewLocation(Apitron.PDF.Rasterizer.Rectangle destinationInfo)
        {
            if (destinationInfo == null)
            {
                this.PageScroller.ScrollToTop();
                return;
            }
            double value = this.ZoomSlider.Value;
            double scale = value;

            double horizontalScale = this.PageScroller.ViewportWidth / this.PageImage.ActualWidth;
            double verticalScale = this.PageScroller.ViewportHeight / this.PageImage.ActualHeight;

            if (destinationInfo.Bottom != 0 && destinationInfo.Right != this.PageImage.ActualWidth)
            {
                double expectedHScale = this.PageScroller.ViewportWidth / destinationInfo.Height;
                double expectidVScale = this.PageScroller.ViewportHeight / destinationInfo.Width;
                horizontalScale = expectedHScale;
                verticalScale = expectidVScale;

                scale = Math.Min(verticalScale, horizontalScale);
                this.ZoomSlider.Value = scale;
            }

            this.PageScroller.ScrollToHorizontalOffset(destinationInfo.Left * scale);
            this.PageScroller.ScrollToVerticalOffset((this.PageImage.ActualHeight - destinationInfo.Top) * scale);

        }

        #endregion

        #region Event Handlers

        private void OnCloseFileClick(object sender, RoutedEventArgs e)
        {
            Close();
            //(Application.Current.Windows[0] as Views.Windows.MainWindow).RootNavigation.
            //   PageService.GetPage<BooksPage>()._NavigationFrame.Navigate(new ListBooksPage());

        }

        private void OnLinkClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Link link = (Link)((Button)sender).DataContext;
            this.document.Document.Navigator.GoToLink(link);

            this.destinationRectangle = link.GetDestinationRectangle((int)(this.document.Page.Width * this.GlobalScale), (int)(this.document.Page.Height * this.GlobalScale), null);
        }


        private async void OnNavigationButtonClick(object sender, RoutedEventArgs e)
        {
            Button source = (Button)e.Source;
            Document doc = document.Document;
            DocumentNavigator navigator = doc == null ? null : doc.Navigator;
            if (doc == null || navigator == null)
            {
                return;
            }
            switch ((string)source.CommandParameter)
            {
                case "Next":
                    navigator.MoveForward();
                    break;
                case "Prev":
                    navigator.MoveBackward();
                    break;
                case "First":
                    navigator.Move(0, Origin.Begin);
                    break;
                case "Last":
                    navigator.Move(0, Origin.End);
                    break;
                default:
                    return;
            }
            if (!AppConfig.IsСurrentUserDefault() && _pdfReadingService != null)
            {
                if((doc.Pages.Count*0.9)< doc.Pages.IndexOf(navigator.CurrentPage))
                {
                    await _pdfReadingService.AccomplitAsync(AppConfig.User.Id, _pdfReading.Id);

                }
            }
            this.destinationRectangle = null;
        }

        private void OnZoomChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.UpdateImageZoom();
        }


        private void DocumentOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Page")
            {
                task = new Task(UpdatePageView);
                task.Start();
            }
        }

        #endregion

        #region INavigationWindow methods

        public Frame GetFrame()
                => RootFrame;

        public INavigation GetNavigation()
            => null;

        public bool Navigate(Type pageType)
            => false;

        public void SetPageService(IPageService pageService)
        { }

        public void ShowWindow()
        {
            Show();
        }

        public void CloseWindow()
        {
            Hide();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        #endregion
    }
}
