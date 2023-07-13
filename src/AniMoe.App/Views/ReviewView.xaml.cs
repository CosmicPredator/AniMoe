using AniMoe.App.ViewModels;
using AniMoe.Parsers;
using CommunityToolkit.WinUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AniMoe.App.Views
{
    public sealed partial class ReviewView : Page
    {
        public DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        private ReviewViewModel ViewModel;
        
        private int ReviewId;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ReviewId = (int)e.Parameter;
            ViewModel = new(ReviewId, ReviewWebView, dispatcherQueue);
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ReviewWebView.Close();
            GC.Collect();
            base.OnNavigatedFrom(e);
        }

        public ReviewView()
        {
            this.InitializeComponent();
        }
    }
}
