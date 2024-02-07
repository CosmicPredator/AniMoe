using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using CommunityToolkit.WinUI;
using AniMoe.App.ViewModels;
using AniMoe.Parsers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using CommunityToolkit.Labs.WinUI;
using Microsoft.UI.Xaml.Media.Animation;
using AniMoe.App.Controls;
using Windows.Media;
using AniMoe.App.Controls.CharacterViewControls;
using CommunityToolkit.WinUI.UI;
using System.Diagnostics;

namespace AniMoe.App.Views
{
    public sealed partial class CharacterView : Page, IDisposable
    {
        private DispatcherQueue dispatchQueue = DispatcherQueue.GetForCurrentThread();
        public CharacterViewModel ViewModel;
        public int CharId;
        public IMdToHtmlParser mdToHtmlParser = App.Current.Services.GetRequiredService<IMdToHtmlParser>();
        DispatcherQueueTimer timer = DispatcherQueue.GetForCurrentThread().CreateTimer();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = new((int)e.Parameter);
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) => Dispose();

        public CharacterView()
        {
            this.InitializeComponent();
        }

        private void MasterScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            SegmentedBox.SelectionChanged += SegmentedBox_SelectionChanged;
            SegmentedBox.SelectedIndex = 0;
            //await DescriptionWebView.EnsureCoreWebView2Async();
            //await Task.Delay(500);
            //DescriptionWebView.NavigateToString(
            //    mdToHtmlParser.Convert(ViewModel.Model.Data.Character.Description)
            //);
        }

        //private async void DescriptionWebView_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        //{
        //    string jsx = "document.body.offsetHeight";
        //    string heightString = await DescriptionWebView.ExecuteScriptAsync(jsx);
        //    Debug.WriteLine(heightString);

        //    if( double.TryParse(heightString, out double height) )
        //    {
        //        WebGrid.Height = height >= 800 ? height : height / 4;
        //    }

        //    string js = @"
        //        var summaryTags = document.getElementsByTagName('summary');
        //        for (var i = 0; i < summaryTags.length; i++) {
        //            summaryTags[i].addEventListener('click', function() {
        //                window.chrome.webview.postMessage('summaryClicked');
        //            });
        //        }
        //        function calculateContentHeight() {
        //            var body = document.body;
        //            var html = document.documentElement;
        //            var height = Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight);
        //            window.chrome.webview.postMessage(height.toString());
        //        }
        //        calculateContentHeight();
        //    ";
        //    await DescriptionWebView.ExecuteScriptAsync(js);
        //    DescriptionWebView.WebMessageReceived += DescriptionWebView_WebMessageReceived;
        //}

        //private void DescriptionWebView_WebMessageReceived(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs args)
        //{
        //    string message = args.TryGetWebMessageAsString();
        //    if( !string.IsNullOrEmpty(message) )
        //    {
        //        if( message == "summaryClicked" )
        //        {
        //            // Update WebView2 height when summary tag is clicked
        //            UpdateWebViewHeight();
        //        }
        //        else if( double.TryParse(message, out double contentHeight) )
        //        {
        //            // Use the content height value
        //            WebGrid.Height = contentHeight;
        //        }
        //    }
        //}

        //private async void UpdateWebViewHeight()
        //{
        //    // Inject JavaScript code to recalculate content height after summary tag is clicked
        //    string js = "calculateContentHeight();";
        //    await DescriptionWebView.ExecuteScriptAsync(js);
        //}

        //private void DescriptionWebView_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        //{
        //    try
        //    {
        //        DescriptionWebView.IsHitTestVisible = false;
        //        timer.Debounce(() => {
        //            DescriptionWebView.IsHitTestVisible = true;
        //        }, TimeSpan.FromMilliseconds(500));
        //    }
        //    catch( Exception ex )
        //    {
        //        Log.Error(ex, "Some Error Occured");
        //    }
        //}

        private void SegmentedBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Log.Information("Hello");
            Segmented nv = sender as Segmented;
            SegmentedItem item = nv.SelectedValue as SegmentedItem;
            var navOptions = new FrameNavigationOptions
            {
                IsNavigationStackEnabled = false,
                TransitionInfoOverride = new EntranceNavigationTransitionInfo()
            };

            switch( item.Name )
            {
                case "AnimePage":
                    Dictionary<string, dynamic> keyValuePairs = new Dictionary<string, dynamic>();
                    keyValuePairs["ViewModel"] = ViewModel.Model.Data.Character.Media;
                    keyValuePairs["Type"] = "ANIME";
                    keyValuePairs["RootFrame"] = Frame;
                    ChildFrame.NavigateToType(typeof(AnimeListControl), keyValuePairs, navOptions);
                    break;

                case "MangaPage":
                    Dictionary<string, dynamic> keyValuePairsManga = new Dictionary<string, dynamic>();
                    keyValuePairsManga["ViewModel"] = ViewModel.Model.Data.Character.Media;
                    keyValuePairsManga["Type"] = "MANGA";
                    keyValuePairsManga["RootFrame"] = Frame;
                    ChildFrame.NavigateToType(typeof(AnimeListControl), keyValuePairsManga, navOptions);
                    break;
            }
        }

        public void Dispose()
        {
            //if (DescriptionWebView is not null) DescriptionWebView.Close();
            GC.Collect();
        }
    }
}
