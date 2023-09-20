using AniMoe.App.ViewModels;
using AniMoe.Parsers;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.UI;
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
    public sealed partial class ReviewView : Page, IDisposable
    {
        public DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        private ReviewViewModel ViewModel;
        DispatcherQueueTimer timer = DispatcherQueue.GetForCurrentThread().CreateTimer();

        private int ReviewId;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ReviewId = (int)e.Parameter;
            ViewModel = new(ReviewId, ReviewWebView, dispatcherQueue);
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) => Dispose();

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
            string jsx = "Math.max(document.documentElement.scrollHeight, document.body.scrollHeight);";
            string heightString = await ReviewWebView.ExecuteScriptAsync(jsx);

            if( double.TryParse(heightString, out double height) )
            {
                WebGrid.Height = height;
            }
            string js = @"
                var summaryTags = document.getElementsByTagName('summary');
                for (var i = 0; i < summaryTags.length; i++) {
                    summaryTags[i].addEventListener('click', function() {
                        window.chrome.webview.postMessage('summaryClicked');
                    });
                }
                function calculateContentHeight() {
                    var body = document.body;
                    var html = document.documentElement;
                    var height = Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight);
                    window.chrome.webview.postMessage(height.toString());
                }
                calculateContentHeight();
            ";
            await ReviewWebView.ExecuteScriptAsync(js);
            ReviewWebView.WebMessageReceived += ReviewWebView_WebMessageReceived;
        }

        private void ReviewWebView_WebMessageReceived(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs args)
        {
            string message = args.TryGetWebMessageAsString();
            if( !string.IsNullOrEmpty(message) )
            {
                if( message == "summaryClicked" )
                {
                    // Update WebView2 height when summary tag is clicked
                    UpdateWebViewHeight();
                }
                else if( double.TryParse(message, out double contentHeight) )
                {
                    // Use the content height value
                    WebGrid.Height = contentHeight;
                }
            }
        }

        private async void UpdateWebViewHeight()
        {
            // Inject JavaScript code to recalculate content height after summary tag is clicked
            string js = "calculateContentHeight();";
            await ReviewWebView.ExecuteScriptAsync(js);
        }

        private void ReviewWebView_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            ReviewWebView.IsHitTestVisible = false;
            timer.Debounce(() => {
                ReviewWebView.IsHitTestVisible = true;
            }, TimeSpan.FromMilliseconds(500));
        }

        public void Dispose()
        {
            if (ReviewWebView is not null) ReviewWebView.Close();
            GC.Collect();
        }
    }
}
