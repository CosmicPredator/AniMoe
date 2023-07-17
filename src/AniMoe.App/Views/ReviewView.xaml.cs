using AniMoe.App.ViewModels;
using AniMoe.Parsers;
using CommunityToolkit.WinUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AniMoe.App.Views
{
    public sealed partial class ReviewView : Page
    {
        public DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        private ReviewViewModel ViewModel;
        
        private int ReviewId;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ReviewId = (int)e.Parameter;
            ViewModel = new(ReviewId, ReviewWebView, dispatcherQueue);
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ReviewWebView.Close();
            GC.Collect();
            base.OnNavigatedFrom(e);
        }

        public ReviewView()
        {
            this.InitializeComponent();
        }

        private async void ReviewWebView_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            if( args.Uri != null && args.IsRedirected )
            {
                args.Cancel = true;
                await Windows.System.Launcher.LaunchUriAsync(new Uri(args.Uri));
            }
            await SetWebViewHeight();
            ReviewWebView.SizeChanged += ReviewWebView_SizeChanged;
            MasterScrollViewer.ViewChanged += MasterScrollViewer_ViewChanged;
        }

        private async void ReviewWebView_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            await SetWebViewHeight();
            ReviewWebView.SizeChanged += ReviewWebView_SizeChanged;
            MasterScrollViewer.ViewChanged += MasterScrollViewer_ViewChanged;
        }

        private void MasterScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if( scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight)
            {
                ScoreGrid.Visibility = Visibility.Visible;
                MasterScrollViewer.ViewChanged -= MasterScrollViewer_ViewChanged;
            }
        }

        private async void ReviewWebView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            await SetWebViewHeight();
        }

        private async Task SetWebViewHeight()
        {
            string js = "Math.max(document.documentElement.scrollHeight, document.body.scrollHeight);";
            string heightString = await ReviewWebView.ExecuteScriptAsync(js);

            if( double.TryParse(heightString, out double height) )
            {
                WebGrid.Height = height;
            }
        }
    }
}
