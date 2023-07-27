using AniMoe.App.Models.AnimeListModels;
using AniMoe.App.Services;
using AniMoe.App.ViewModels;
using CommunityToolkit.WinUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AniMoe.App.Views
{
    public sealed partial class AnimeListView : Page
    {
        private AnimeListViewModel ViewModel 
            = App.Current.Services.GetRequiredService<AnimeListViewModel>();
        public IEnumerable<int> items = Enumerable.Range(0, 30);
        DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        public AnimeListView()
        {
            this.InitializeComponent();
            //ViewModel = new AnimeListViewModel();
            DataContext = ViewModel;
            TitleText.Text = Environment.Version.ToString();
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

        private void DownloadCoverImage(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            MenuFlyoutItem flyout = sender as MenuFlyoutItem;
            Media md = (Media)flyout.Tag;
            Task.Run(async () =>
            {
                IRequestHandler downloader = App.Current.Services.GetRequiredService<IRequestHandler>();
                await downloader.DownloadCoverImage(
                    md.CoverImage.ExtraLarge.OriginalString,
                    md.Id,
                    md.Title.UserPreferred
                );
            });
        }

        private void CopyUrl(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            MenuFlyoutItem flyout = sender as MenuFlyoutItem;
            IClipCopier copier = App.Current.Services.GetRequiredService<IClipCopier>();
            copier.CopyToClipboard(flyout.Tag.ToString());
        }

        private void UrlLauncher(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            MenuFlyoutItem flyout = sender as MenuFlyoutItem;
            Task.Run(async () =>
            {
                await dispatcherQueue.EnqueueAsync(async () =>
                {
                    await Windows.System.Launcher.LaunchUriAsync(
                        new Uri(flyout.Tag.ToString())
                    );
                });
            });
        }

        private void MasterGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Grid g = sender as Grid;
            if( e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse )
            {
                var properties = e.GetCurrentPoint(this).Properties;
                if( properties.IsLeftButtonPressed )
                {
                    Frame.Navigate(typeof(MediaView), (int)g.Tag, new DrillInNavigationTransitionInfo());
                }
            }
        }
    }
}
