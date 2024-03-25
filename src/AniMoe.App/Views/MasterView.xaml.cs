using AniMoe.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Dispatching;
using System.Diagnostics;
using System.Linq;
using Microsoft.UI.Xaml;
using System;

namespace AniMoe.App.Views
{
    public sealed partial class MasterView : Page
    {
        public SplashViewModel ViewModel;
        DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        public MasterView()
        {
            ViewModel = App.Current.Services.GetRequiredService<SplashViewModel>();
            this.InitializeComponent();
            DataContext = ViewModel;
        }

        private void MasterNavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if( PrimaryFrame.CanGoBack )
            {
                PrimaryFrame.GoBack();
            }    
        }

        private void MasterNavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var selected = sender.SelectedItem as NavigationViewItem;
            Debug.WriteLine(selected.Name);
            switch (selected.Name)
            {
                case "HomeSection":
                    PrimaryFrame.Navigate(typeof(NotImplementedPage), null);
                    break;
                case "AnimeListSection":
                    PrimaryFrame.Navigate(typeof(AnimeListView), null);
                    break;
                case "MangaListSection":
                    PrimaryFrame.Navigate(typeof(MangaListView), null);
                    break;
                case "ExploreSection":
                    PrimaryFrame.Navigate(typeof(ExploreView), null);
                    break;
                case "NotificationSection":
                    NotificationBadge.Opacity = 0;
                    PrimaryFrame.Navigate(typeof(NotImplementedPage), null);
                    break;
                case "UserSection":
#if DEBUG
                    PrimaryFrame.Navigate(typeof(UserView), null);
#elif RELEASE
                    PrimaryFrame.Navigate(typeof(NotImplementedPage), null);
#endif
                    break;
                case "SettingsItem":
                    PrimaryFrame.Navigate(typeof(SettingsView), null);
                    break;
            }
        }

        private void PrimaryFrame_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            Debug.WriteLine(e.Content);
            switch( e.Content )
            {
                case AnimeListView:
                    MasterNavView.SelectedItem = MasterNavView.MenuItems.Where(
                        x => (x as NavigationViewItem).Content.ToString() == "Anime List"
                    ).First();
                    break;
                case MangaListView:
                    MasterNavView.SelectedItem = MasterNavView.MenuItems.Where(
                        x => (x as NavigationViewItem).Content.ToString() == "Manga List"
                    ).First();
                    break;
                case ExploreView:
                    MasterNavView.SelectedItem = MasterNavView.MenuItems.Where(
                        x => (x as NavigationViewItem).Content.ToString() == "Explore"
                    ).First();
                    break;
                //case SettingsView:
                //    MasterNavView.SelectedItem = MasterNavView.MenuItems.Where(
                //        x => (x as NavigationViewItem).Name.ToString() == "SettingsItem"
                //    ).First();
                //    break;
            }
            MasterNavView.IsBackEnabled = PrimaryFrame.CanGoBack;
        }

        private void MasterNavView_Loaded(object sender, RoutedEventArgs e)
        {
            RootGrid.SizeChanged += RootGrid_SizeChanged;
            RepositionTitleBar();
            PrimaryFrame.Navigate(typeof(AnimeListView), null);
        }

        private void MasterNavView_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
        {
            var window = (Application.Current as App)?.m_window as RootWindow;
            Thickness thick = new Thickness()
            {
                Left = MasterNavView.CompactPaneLength * (MasterNavView.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
                Top = 5
            };
            PrimaryFrame.Margin = new Thickness()
            {
                Top = MasterNavView.DisplayMode == NavigationViewDisplayMode.Minimal ? 30 : 0
            };
            window.ChangeTitleBarThickness(thick);
        }

        private void RootGrid_SizeChanged(object sender, SizeChangedEventArgs e)
            => RepositionTitleBar();

        private void RepositionTitleBar()
        {
            if( RootGrid.ActualWidth < 1200 )
            {
                MasterNavView.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftMinimal;
            }
            else
            {
                MasterNavView.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftCompact;
            }
        }
    }
}
