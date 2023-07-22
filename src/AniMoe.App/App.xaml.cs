using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using AniMoe.App.Views;
using AniMoe.App.Helpers;
using Microsoft.Extensions.DependencyInjection;
using AniMoe.App.ViewModels;
using AniMoe.App.Models.AnimeListModels;
using Windows.Web.Http;
using AniMoe.App.Dialogs;
using AniMoe.App.Services;
using Microsoft.Extensions.Configuration;
using AniMoe.App.Models.MasterModel;
using Microsoft.UI.Xaml.Media.Animation;
using AniMoe.Parsers;
using Microsoft.Web.WebView2.Core;
using Microsoft.UI.Dispatching;
using System.Net.Cache;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using AniMoe.App.Models.MediaListStatusModel;
using Serilog;

namespace AniMoe.App
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; }
        public App()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .CreateLogger();
            Log.Information("Logging Services Started");
            Services = ConfigureServices();
            this.InitializeComponent();
        }

        public new static App Current => (App)Application.Current;

        public Window m_window;

        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddScoped<IResourceLocator, ResourceLocator>()
                .AddScoped<ILocalSettings, LocalSettings>()
                .AddScoped<IClipCopier, ClipCopier>()
                .AddScoped<INotificationService, NotificationService>()
                .AddSingleton<MasterViewModel>()
                .AddSingleton<MediaListStatusModel>()
                .AddSingleton(DispatcherQueue.GetForCurrentThread())
                .AddSingleton<DrillInNavigationTransitionInfo>()
                .AddSingleton<IMdToHtmlParser, MdToHtmlParser>()
                .AddSingleton<AnimeListViewModel>()
                .AddSingleton<MangaListViewModel>()
                .AddSingleton<ExploreViewModel>();

            services.AddHttpClient<IRequestHandler, RequestHandler>();
            Log.Information("All Dependencies Hosted");
            return services.BuildServiceProvider();
        }

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {

            var mainInstance = Microsoft.Windows.AppLifecycle.AppInstance.FindOrRegisterForKey("main");

            if (!mainInstance.IsCurrent)
            {
                var activatedArgs = 
                    Microsoft.Windows
                    .AppLifecycle.AppInstance
                    .GetCurrent().GetActivatedEventArgs();
                var token = (ProtocolActivatedEventArgs)activatedArgs.Data;
                var slicedToken = SlicedToken(token.Uri.ToString());
                Log.Debug($"Auth Token: {slicedToken}");
                ILocalSettings settings = App.Current.Services.GetService<ILocalSettings>();
                settings.WriteSettings("accessToken", slicedToken);
                await mainInstance.RedirectActivationToAsync(activatedArgs);
                Process.GetProcessById((int)mainInstance.ProcessId).Kill();
                Log.Information("Authorization handled successfully and accessToken was saved");
            }
            m_window = new RootWindow();
            m_window.ExtendsContentIntoTitleBar = true;
            m_window.Activate();
            Log.Information("RootWindow initialized");
        }

        private string SlicedToken(string rawToken)
        {
            string token = rawToken.Replace("animoe:/auth#access_token=", "");
            token = token.Split('&')[0];
            return token;
        }
    }
}
