using AniMoe.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Dispatching;
using System.Diagnostics;
using System.Linq;
using Microsoft.UI.Xaml;

namespace AniMoe.App.Views
{
    public sealed partial class MasterView : Page
    {
        public MasterViewModel ViewModel;
        private DrillInNavigationTransitionInfo NavAnimation =
            App.Current.Services.GetRequiredService<DrillInNavigationTransitionInfo>();
        DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        public MasterView()
        {
            ViewModel = App.Current.Services.GetRequiredService<MasterViewModel>();
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
            if( (string)selected.Content == "Anime List" )
            {
                PrimaryFrame.Navigate(typeof(AnimeListView), null, NavAnimation);
            }
            else if( (string)selected.Content == "Manga List" )
            {
                PrimaryFrame.Navigate(typeof(MangaListView), null, NavAnimation);
            }
            else if( (string)selected.Content == "Explore" )
            {
                PrimaryFrame.Navigate(typeof(ExploreView), null, NavAnimation);
            }
            else if( (string)selected.Content == "Home" )
            {
                PrimaryFrame.Navigate(typeof(ReviewView), null, NavAnimation);
            }
        }

        private void PrimaryFrame_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
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
            }
            MasterNavView.IsBackEnabled = PrimaryFrame.CanGoBack;
        }

        private void MasterNavView_Loaded(object sender, RoutedEventArgs e)
        {
            PrimaryFrame.Navigate(typeof(AnimeListView), null, NavAnimation);
        }
    }
}
