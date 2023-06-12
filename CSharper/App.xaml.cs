using CSharper.Models;
using CSharper.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace CSharper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
            .ConfigureServices((context, services) =>
            {
                // App Host
                services.AddHostedService<ApplicationHostService>();

                // Page resolver service
                services.AddSingleton<IPageService, PageService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window with navigation
                services.AddScoped<Views.Windows.MainWindow>();
                services.AddScoped<ViewModels.MainWindowViewModel>();

                // Admin window with navigation
                services.AddScoped<Views.Windows.AdminWindow>();
                services.AddScoped<ViewModels.AdminWindowViewModel>();

                // Views and ViewModels
                services.AddScoped<Views.Pages.HomePage>();
                services.AddScoped<ViewModels.HomeViewModel>();
                services.AddScoped<Views.Pages.DataPage>();
                services.AddScoped<ViewModels.DataViewModel>();
                services.AddScoped<Views.Pages.SettingsPage>();
                services.AddScoped<ViewModels.SettingsViewModel>();
              
                services.AddScoped<Views.Pages.BooksPage>();
                services.AddScoped<ViewModels.BooksViewModel>();
                services.AddScoped<Views.Pages.ListBooksPage>();
                services.AddScoped<ViewModels.ListBooksViewModel>();
                services.AddScoped<Views.Pages.PdfViewerPage>();
                services.AddScoped<ViewModels.PdfViewerViewModel>();


                // Admin Views and ViewModels
                services.AddScoped<Views.Pages.AdminPages.AddSubjectPage>();
                services.AddScoped<ViewModels.AdminViewModels.AddSubjectViewModel>();
                services.AddScoped<Views.Pages.AdminPages.AddLessonPage>();
                services.AddScoped<ViewModels.AdminViewModels.AddLessonViewModel>();
                services.AddScoped<Views.Pages.AdminPages.AddBookPage>();
                services.AddScoped<ViewModels.AdminViewModels.AddBookViewModel>();
                services.AddScoped<Views.Pages.AdminPages.AddVideoPage>();
                services.AddScoped<ViewModels.AdminViewModels.AddVideoViewModel>();
                services.AddScoped<Views.Pages.AdminPages.AddArticlePage>();
                services.AddScoped<ViewModels.AdminViewModels.AddArticleViewModel>();
                services.AddScoped<Views.Pages.AdminPages.AddAssignmentPage>();
                services.AddScoped<ViewModels.AdminViewModels.AddAssignmentViewModel>();
                services.AddScoped<Views.Pages.AdminPages.EditSubjectPage>();
                services.AddScoped<ViewModels.AdminViewModels.EditSubjectViewModel>();
                services.AddScoped<Views.Pages.AdminPages.EditLessonPage>();
                services.AddScoped<ViewModels.AdminViewModels.EditLessonViewModel>();
                services.AddScoped<Views.Pages.AdminPages.EditBookPage>();
                services.AddScoped<ViewModels.AdminViewModels.EditBookViewModel>();
                services.AddScoped<Views.Pages.AdminPages.EditVideoPage>();
                services.AddScoped<ViewModels.AdminViewModels.EditVideoViewModel>();
                services.AddScoped<Views.Pages.AdminPages.EditArticlePage>();
                services.AddScoped<ViewModels.AdminViewModels.EditArticleViewModel>();
                services.AddScoped<Views.Pages.AdminPages.EditAssignmentPage>();
                services.AddScoped<ViewModels.AdminViewModels.EditAssignmentViewModel>();

                // Configuration
                services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
            }).Build();

        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}