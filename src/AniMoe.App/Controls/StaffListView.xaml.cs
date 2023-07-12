using AniMoe.App.Models.StaffListModel;
using AniMoe.App.ViewModels;
using AniMoe.App.Views;
using CommunityToolkit.Common.Collections;
using CommunityToolkit.WinUI;
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
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace AniMoe.App.Controls
{
    public sealed partial class StaffListView : Page
    {
        public NavObject NavArgs { get; set; }
        public MediaViewViewModel ViewModel { get; set; }
        public int MediaId { get; set; }
        private IncrementalLoadingCollection<StaffListLoader, Edge> Collection;
        private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavArgs = e.Parameter as NavObject;
            ViewModel = NavArgs.Vm;
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }

        public StaffListView()
        {
            this.InitializeComponent();
        }

        private void StaffListItRepeater_Loaded(object sender, RoutedEventArgs e)
        {
            
            MediaId = ViewModel.Model.Data.Media.Id;
            Collection = new IncrementStaffList(MediaId, new StaffListLoader());
            dispatcherQueue.TryEnqueue(DispatcherQueuePriority.High, () =>
            {
                StaffListItRepeater.ItemsSource = Collection;
            });
            if( ItScrollViewer != null )
            {
                ItScrollViewer.ViewChanged += ItScrollViewer_ViewChanged;
            }
            Task.Run(async () =>
            {
                await dispatcherQueue.EnqueueAsync(async () =>
                {
                    await Collection.LoadMoreItemsAsync(25);
                });
            });
        }

        private async void ItScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if( scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 20.0 )
            {
                if( !Collection.IsLoading )
                {
                    await Collection.LoadMoreItemsAsync(25);
                }
            }
        }
    }

    public class IncrementStaffList: IncrementalLoadingCollection<StaffListLoader, Edge>
    {
        public int MediaId { get; set; }
        public readonly StaffListLoader Loader;
        public IncrementStaffList(int mediaId, StaffListLoader loader)
        {
            MediaId = mediaId;
            Loader = loader;
            Loader.MediaId = MediaId;
        }
        protected override Task<IEnumerable<Edge>> LoadDataAsync(CancellationToken cancellationToken)
        {
            return Loader.GetPagedItemsAsync(1, 1);
        }
    }

    public class StaffListLoader : IIncrementalSource<Edge>
    {
        public int MediaId { get; set; }
        public int pageNum { get; set; } = 1;
        public bool HasNextPage { get; set; } = true;

        public async Task<IEnumerable<Edge>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            List<Edge> items = new List<Edge>();
            if ( HasNextPage )
            {
                StaffListModel Model = await Initialize.FetchData(MediaId, pageNum);
                items = Model.Data.Media.Staff.Edges;
                HasNextPage = Model.Data.Media.Staff.PageInfo.HasNextPage;
                pageNum++;
            }
            return items;
        }
    }
}
