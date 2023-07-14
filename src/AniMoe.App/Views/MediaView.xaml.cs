using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using AniMoe.App.ViewModels;
using AniMoe.App.Controls;
using CommunityToolkit.Labs.WinUI;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Media;
using System.Diagnostics;
using AniMoe.Parsers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;
using Windows.UI.WebUI;
using Windows.UI;
using Microsoft.UI.Dispatching;
using System.Threading.Tasks;
using Windows.Web.UI.Interop;
using Microsoft.UI;
using Windows.Web.UI;
using AniMoe.App.Dialogs;
using Windows.Graphics.Display;

namespace AniMoe.App.Views
{
    public sealed partial class MediaView : Page
    {
        private MediaViewViewModel ViewModel;
        private int MediaId;
        private Color BgColor;
        DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MediaId = (int)e.Parameter;
            ViewModel = new(MediaId, dispatcherQueue);
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            GC.Collect();
            base.OnNavigatedFrom(e);
        }

        public MediaView()
        {
            this.InitializeComponent();
        }

        public void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Segmented nv = sender as Segmented;
            SegmentedItem item = nv.SelectedValue as SegmentedItem;
            var navOptions = new FrameNavigationOptions
            {
                IsNavigationStackEnabled = false,
                TransitionInfoOverride = new EntranceNavigationTransitionInfo()
            };

            switch( item.Name )
            {
                case "OverviewPage":
                    MediaFrame.NavigateToType(typeof(MediaOverviewView), new NavObject
                    {
                        Vm = ViewModel,
                        RootFrame = this.Frame
                    }, navOptions);
                    break;

                case "CharactersPage":
                    MediaFrame.NavigateToType(typeof(CharacterListView), new NavObject
                    {
                        Vm = ViewModel,
                        RootFrame = this.Frame
                    }, navOptions);
                    break;

                case "StaffsPage":
                    MediaFrame.NavigateToType(typeof(StaffListView), new NavObject
                    {
                        Vm = ViewModel,
                        RootFrame = this.Frame
                    }, navOptions);
                    break;
                case "StatsPage":
                    MediaFrame.NavigateToType(typeof(StatsPage), new NavObject
                    {
                        Vm = ViewModel,
                        RootFrame = this.Frame
                    }, navOptions);
                    break;
                case "ReviewsPage":
                    MediaFrame.NavigateToType(typeof(ReviewListView), new NavObject
                    {
                        Vm = ViewModel,
                        RootFrame = this.Frame
                    }, navOptions);
                    break;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            DesExpander.IsExpanded = !DesExpander.IsExpanded;
        }

        private async Task<double> GetWebViewContentHeightAsync(WebView2 webView)
        {
            while( webView.CoreWebView2 == null )
            {
                await Task.Delay(10);
            }
            var scrollHeightScript = "Math.max(document.documentElement.scrollHeight, document.body.scrollHeight);";
            var scrollHeight = await webView.CoreWebView2.ExecuteScriptAsync(scrollHeightScript);

            if( double.TryParse(scrollHeight, out double height) )
            {
                return height;
            }

            return 0;
        }

        private void ContentGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 1000 )
            {
                Debug.WriteLine(e.NewSize.Width.ToString());
                VisualStateManager.GoToState(this, "DefLayout", false);
            }
        }
    }
    public class NavObject
    {
        public MediaViewViewModel Vm { get; set; }
        public Frame RootFrame { get; set; }
    }
}
