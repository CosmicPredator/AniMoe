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

        public void Dispose()
        {
            GC.Collect();
        }

        private async void MarkdownTextBlock_LinkClicked(object sender, CommunityToolkit.WinUI.UI.Controls.LinkClickedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(
                    new Uri(e.Link));
        }
    }
}
