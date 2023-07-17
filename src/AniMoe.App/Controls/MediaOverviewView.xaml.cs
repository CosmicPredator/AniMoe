using AniMoe.App.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Reflection.Emit;
using LiveChartsCore.Kernel.Sketches;
using Windows.UI;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.Measure;
using Microsoft.UI;
using AniMoe.App.Models.MediaModel;
using AniMoe.App.ViewModels;
using System.Threading.Tasks;
using AniMoe.App.Views;
using Microsoft.UI.Xaml.Media.Animation;
using System.Collections.ObjectModel;
using Microsoft.UI.Dispatching;
using Serilog;

namespace AniMoe.App.Controls
{
    public sealed partial class MediaOverviewView : Page
    {
        public NavObject NavArgs { get; set; }
        public MediaViewViewModel ViewModel { get; set; }
        private bool IsSpoilerTagsVisible = true;
        DispatcherQueue dispatcherQueue 
            = App.Current.Services.GetRequiredService<DispatcherQueue>();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavArgs = e.Parameter as NavObject;
            ViewModel = NavArgs.Vm;
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }

        public MediaOverviewView()
        {
            this.InitializeComponent();
        }
        public void CardOnHover(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Translation = new System.Numerics.Vector3(0, -5, 30);
        }

        public void CardOffHover(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Translation = new System.Numerics.Vector3(0, 0, 0);
        }

        private async void DownloadCoverImage(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            MenuFlyoutItem flyout = sender as MenuFlyoutItem;
            MediaRecommendation node = (MediaRecommendation)flyout.Tag;
            IRequestHandler downloader = App.Current.Services.GetRequiredService<IRequestHandler>();
            await downloader.DownloadCoverImage(
                node.CoverImage.ExtraLarge.OriginalString,
                node.Id,
                node.Title.UserPreferred
            );
        }

        private void CopyUrl(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem flyout = sender as MenuFlyoutItem;
            IClipCopier copier = App.Current.Services.GetRequiredService<IClipCopier>();
            copier.CopyToClipboard(flyout.Tag.ToString());
        }

        private async void UrlLauncher(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            MenuFlyoutItem flyout = sender as MenuFlyoutItem;
            await Windows.System.Launcher.LaunchUriAsync(
                    new Uri(flyout.Tag.ToString())
            );
        }

        private void RelationCard_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Grid gd = sender as Grid;
            if (e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse )
            {
                var properties = e.GetCurrentPoint(this).Properties;
                if( properties.IsLeftButtonPressed )
                {
                    NavArgs.RootFrame.Navigate(typeof(MediaView), (int)gd.Tag, new DrillInNavigationTransitionInfo());
                }
            }
        }

        private void TagSpoilerButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherQueue.TryEnqueue(() =>
            {
                if( !IsSpoilerTagsVisible )
                {
                    TagSpoilerButton.Content = "Show Spoiler Tags";
                    TagsItemRepeater.ItemsSource = new ObservableCollection<Tag>(
                    ViewModel.Model.Data.Media.Tags.Where(
                            x => x.IsMediaSpoiler == false
                        )
                    );
                }else
                {
                    TagSpoilerButton.Content = "Hide Spoiler Tags";
                    TagsItemRepeater.ItemsSource = 
                        new ObservableCollection<Tag>(ViewModel.Model.Data.Media.Tags);
                }
                IsSpoilerTagsVisible = !IsSpoilerTagsVisible;
            });
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TagSpoilerButton.Content = "Show Spoiler Tags";
            TagsItemRepeater.ItemsSource = new ObservableCollection<Tag>(
                ViewModel.Model.Data.Media.Tags.Where(
                        x => x.IsMediaSpoiler == false
                    )
                );
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            DescriptionTextBlock.MaxLines = DescriptionTextBlock.MaxLines == 0 ? 3 : 0;
            (sender as HyperlinkButton).Content = DescriptionTextBlock.MaxLines == 0 ?
                "read less..." : "read more...";
        }
    }
}
