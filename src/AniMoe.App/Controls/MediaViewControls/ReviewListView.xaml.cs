using AniMoe.App.ViewModels;
using AniMoe.App.Views;
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

namespace AniMoe.App.Controls
{
    public sealed partial class ReviewListView : Page
    {

        public NavObject NavArgs { get; set; }
        public MediaViewViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavArgs = e.Parameter as NavObject;
            ViewModel = NavArgs.Vm;
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }

        public ReviewListView()
        {
            this.InitializeComponent();
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Grid g = sender as Grid;
            if( e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse )
            {
                var properties = e.GetCurrentPoint(this).Properties;
                if( properties.IsLeftButtonPressed )
                {
                    NavArgs.RootFrame.Navigate(typeof(ReviewView), Convert.ToInt32(g.Tag), new DrillInNavigationTransitionInfo());
                }
            }
        }
    }
}
