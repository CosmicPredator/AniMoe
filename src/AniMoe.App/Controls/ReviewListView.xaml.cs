using AniMoe.App.ViewModels;
using AniMoe.App.Views;
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
    }
}
