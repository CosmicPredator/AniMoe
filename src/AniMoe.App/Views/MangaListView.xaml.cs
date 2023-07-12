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
using AniMoe.App.ViewModels;
using AniMoe.App.Models.MangaListModel;
using AniMoe.App.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Microsoft.UI.Xaml.Media.Animation;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using CommunityToolkit.WinUI;
using System;

namespace AniMoe.App.Views
{
    public sealed partial class MangaListView : Page
    {
        public MangaListViewModel ViewModel 
            = App.Current.Services.GetRequiredService<MangaListViewModel>();
        public List<int> items = Enumerable.Range(0, 30).ToList();
        DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        public MangaListView()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
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

        private async void ReloadButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.InitView();
        }

        private void DownloadCoverImage(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            MenuFlyoutItem flyout = sender as MenuFlyoutItem;
            Media md = (Media)flyout.Tag;
            Task.Run(async () =>
            {
                Debug.WriteLine("Triggered");
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
            Frame.Navigate(typeof(MediaView), (int)g.Tag, new DrillInNavigationTransitionInfo());
        }
    }
}
