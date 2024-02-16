using AniMoe.App.ViewModels;
using AniMoe.Parsers;
using CommunityToolkit.WinUI.UI;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AniMoe.App.Views
{
    public sealed partial class StaffView : Page, IDisposable
    {
        public StaffViewModel ViewModel;
        private DispatcherQueue dispatchQueue = DispatcherQueue.GetForCurrentThread();
        private CharacterListIncrementalList collection;
        public int StaffId;
        public IMdToHtmlParser mdToHtmlParser = App.Current.Services.GetRequiredService<IMdToHtmlParser>();
        DispatcherQueueTimer timer = DispatcherQueue.GetForCurrentThread().CreateTimer();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            StaffId = (int)e.Parameter;
            ViewModel = new(StaffId, MasterScrollViewer, CharItemsRepeater);
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) => Dispose();

        public StaffView()
        {
            this.InitializeComponent();
        }

        private void MasterScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStaffList();
        }

        private async void LoadStaffList(bool OnList = false)
        {
            collection = new(StaffId, OnList);
            CharItemsRepeater.ItemsSource = collection;
            if( MasterScrollViewer != null )
            {
                MasterScrollViewer.ViewChanged += MasterScrollViewer_ViewChangedAsync;
            }
            await collection.LoadMoreItemsAsync(25);
        }

        private async void MasterScrollViewer_ViewChangedAsync(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if( scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 30.0 )
            {
                if( !collection.IsLoading )
                {
                    await collection.LoadMoreItemsAsync(25);
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if( (sender as CheckBox).IsChecked == true )
                LoadStaffList(true);
            else if( (sender as CheckBox).IsChecked == false )
                LoadStaffList();

        }

        private void MediaImage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse)
            {
                var properties = e.GetCurrentPoint(this).Properties;
                if (properties.IsLeftButtonPressed)
                {
                    Frame.Navigate(typeof(MediaView), Convert.ToInt32((sender as ImageEx).Tag),
                        new DrillInNavigationTransitionInfo());
                }
            }
        }

        private void CharImage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse)
            {
                var properties = e.GetCurrentPoint(this).Properties;
                if (properties.IsLeftButtonPressed)
                {
                    Frame.Navigate(typeof(CharacterView), Convert.ToInt32((sender as ImageEx).Tag),
                        new DrillInNavigationTransitionInfo());
                }
            }
        }

        public void Dispose()
        {
            GC.Collect();
        }

        private async void MarkdownTextBlock_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(
                    new Uri(e.Link));
        }
    }
}
