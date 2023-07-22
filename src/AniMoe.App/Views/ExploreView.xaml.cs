using AniMoe.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AniMoe.App.Views
{
    public sealed partial class ExploreView : Page
    {
        public ExploreViewModel ViewModel 
            = App.Current.Services.GetRequiredService<ExploreViewModel>();
        public IEnumerable<int> items = Enumerable.Range(0, 30);
        public ExploreView()
        {
            DataContext = ViewModel;
            this.InitializeComponent();
            ViewModel.PassControls(MasterItemsRepeater, ItScrollViewer, SearchBox);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            GC.Collect();
            base.OnNavigatedFrom(e);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Shimmer.ItemsSource = items;
        }

        private void MasterGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Translation = new System.Numerics.Vector3(0, -5, 30);
        }

        private void MasterGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Translation = new System.Numerics.Vector3(0, 0, 0);
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

        private void ReviewGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Grid g = sender as Grid;
            if( e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse )
            {
                var properties = e.GetCurrentPoint(this).Properties;
                if( properties.IsLeftButtonPressed )
                {
                    Frame.Navigate(typeof(ReviewView), Convert.ToInt32(g.Tag), new DrillInNavigationTransitionInfo());
                }
            }
        }
    }
}
