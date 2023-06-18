using CSharper.Views.Windows;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Mvvm.Contracts;

namespace CSharper.Services
{
    /// <summary>
    /// Managed host of the application.
    /// </summary>
    public class ApplicationHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private INavigationWindow _navigationWindow;
        private INavigationWindow _adminWindow;
        private INavigationWindow _readerWindow;

        public ApplicationHostService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await HandleActivationAsync();
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Creates main window during activation.
        /// </summary>
        private async Task HandleActivationAsync()
        {
            await Task.CompletedTask;

            _navigationWindow = (_serviceProvider.GetService(typeof(Views.Windows.MainWindow)) as INavigationWindow)!;
            _adminWindow = (_serviceProvider.GetService(typeof(Views.Windows.AdminWindow)) as INavigationWindow)!;
            _readerWindow = (_serviceProvider.GetService(typeof(Views.Windows.PdfViewerWindow)) as INavigationWindow)!;
            
             _navigationWindow!.ShowWindow();
           
            _navigationWindow.Navigate(typeof(Views.Pages.AutorizationPage));


            await Task.CompletedTask;
        }
    }
}
