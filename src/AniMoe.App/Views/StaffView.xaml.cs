using AniMoe.App.ViewModels;
using AniMoe.Parsers;
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
    public sealed partial class StaffView : Page
    {
        public StaffViewModel ViewModel;
        private DispatcherQueue dispatchQueue = DispatcherQueue.GetForCurrentThread();
        private CharacterListIncrementalList collection;
        public int StaffId;
        public IMdToHtmlParser mdToHtmlParser = App.Current.Services.GetRequiredService<IMdToHtmlParser>();
        DispatcherQueueTimer timer = DispatcherQueue.GetForCurrentThread().CreateTimer();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            StaffId = (int)e.Parameter;
            ViewModel = new(StaffId, MasterScrollViewer, CharItemsRepeater);
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }

        public StaffView()
        {
            this.InitializeComponent();
        }

        private async void DescriptionWebView_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            string jsx = "document.body.offsetHeight";
            string heightString = await DescriptionWebView.ExecuteScriptAsync(jsx);
            Debug.WriteLine(heightString);
            if( double.TryParse(heightString, out double height) )
            {
                WebGrid.Height = height >= 800 ? height : height / 4;
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
            await DescriptionWebView.ExecuteScriptAsync(js);
            DescriptionWebView.WebMessageReceived += DescriptionWebView_WebMessageReceived;
        }

        private async void UpdateWebViewHeight()
        {
            // Inject JavaScript code to recalculate content height after summary tag is clicked
            string js = "calculateContentHeight();";
            await DescriptionWebView.ExecuteScriptAsync(js);
        }

        private void DescriptionWebView_WebMessageReceived(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs args)
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

        private void DescriptionWebView_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                DescriptionWebView.IsHitTestVisible = false;
                timer.Debounce(() => {
                    DescriptionWebView.IsHitTestVisible = true;
                }, TimeSpan.FromMilliseconds(500));
            }
            catch( Exception ex )
            {
                Log.Error(ex, "Some Error Occured");
            }
        }

        private async void MasterScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Model.Data.Staff.Description != null)
            {
                await DescriptionWebView.EnsureCoreWebView2Async();
                await Task.Delay(500);
                DescriptionWebView.NavigateToString(
                    mdToHtmlParser.Convert(ViewModel.Model.Data.Staff.Description)
                );
            }
            LoadStaffList();
        }

        private async void LoadStaffList(bool OnList = false)
        {
            collection = new(StaffId, OnList);
            CharItemsRepeater.ItemsSource = collection;
            if( MasterScrollViewer != null )
            {
                MasterScrollViewer.ViewChanged += MasterScrollViewer_ViewChangedAsync;
            }
            await collection.LoadMoreItemsAsync(25);
        }

        private async void MasterScrollViewer_ViewChangedAsync(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if( scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 30.0 )
            {
                if( !collection.IsLoading )
                {
                    await collection.LoadMoreItemsAsync(25);
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if( (sender as CheckBox).IsChecked == true )
                LoadStaffList(true);
            else if( (sender as CheckBox).IsChecked == false )
                LoadStaffList();

        }
    }
}
