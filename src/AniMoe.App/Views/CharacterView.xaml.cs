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

namespace AniMoe.App.Views
{
    public sealed partial class CharacterView : Page
    {
        private DispatcherQueue dispatchQueue = DispatcherQueue.GetForCurrentThread();
        public CharacterViewModel ViewModel;
        public int CharId;
        public IMdToHtmlParser mdToHtmlParser = App.Current.Services.GetRequiredService<IMdToHtmlParser>();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CharId = 172759;
            ViewModel = new(CharId);
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }

        public CharacterView()
        {
            this.InitializeComponent();
        }

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

        private async void MasterScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            SegmentedBox.SelectionChanged += SegmentedBox_SelectionChanged;
            SegmentedBox.SelectedIndex = 0;
            await DescriptionWebView.EnsureCoreWebView2Async();
            await Task.Delay(500);
            DescriptionWebView.NavigateToString(
                mdToHtmlParser.Convert(ViewModel.Model.Data.Character.Description)
            );
        }

        private async void DescriptionWebView_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            string js = "document.body.offsetHeight";
            string heightString = await DescriptionWebView.ExecuteScriptAsync(js);

            Log.Information(heightString);
            if( double.TryParse(heightString, out double height) )
            {
                WebGrid.Height = height / 3.5;
            }
        }

        private void SegmentedBox_ItemClick(object sender, ItemClickEventArgs e)
        {
        }
    }
}
