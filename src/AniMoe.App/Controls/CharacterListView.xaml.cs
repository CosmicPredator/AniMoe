using AniMoe.App.Models.CharacterListModel;
using AniMoe.App.ViewModels;
using AniMoe.App.Views;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommunityToolkit.Common.Collections;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Animation;
using CommunityToolkit.WinUI.UI.Controls;

namespace AniMoe.App.Controls
{
    public sealed partial class CharacterListView : Page
    {
        public NavObject NavArgs { get; set; }
        public MediaViewViewModel ViewModel { get; set; }
        private List<string> VALanguages { get; set; } = new();
        private IncrementalLoadingCollection<CharacterListLoader, Edge> collection;
        private const double Threshold = 20.0;
        public string MediaType { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavArgs = e.Parameter as NavObject;
            ViewModel = NavArgs.Vm;
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }

        public CharacterListView()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MediaType = ViewModel.Model.Data.Media.Type;
            if (MediaType == "ANIME" )
            {
                foreach( var item in ViewModel.Model.Data.Media.Characters.Edges )
                {
                    foreach( var VA in item.VoiceActors )
                    {
                        if( !VALanguages.Contains(VA.LanguageV2) )
                        {
                            VALanguages.Add(VA.LanguageV2);
                        }
                    }
                }
                LanguageComboBox.SelectionChanged += LanguageComboBox_SelectionChanged;
                LanguageComboBox.ItemsSource = VALanguages;
                LanguageComboBox.SelectedIndex = 0;
            }
            collection = new IncrementLoader(ViewModel.Model.Data.Media.Id,
                             LanguageComboBox == null ? null : LanguageComboBox.SelectedValue.ToString(), 
                             new CharacterListLoader(),
                             MediaType);
            CharacterListItRepeater.ItemsSource = collection;
            if (NavArgs.ParentScrollViewer != null)
            {
                NavArgs.ParentScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
            }
            await collection.LoadMoreItemsAsync(25);
        }

        private async void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (collection != null) collection.Clear();
            collection = new IncrementLoader(ViewModel.Model.Data.Media.Id,
                    LanguageComboBox.SelectedValue.ToString(), new CharacterListLoader(),
                    MediaType);
            CharacterListItRepeater.ItemsSource = collection;
            if (NavArgs.ParentScrollViewer != null)
            {
                NavArgs.ParentScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
            }
            await collection.LoadMoreItemsAsync(25);
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - Threshold)
            {
                if (!collection.IsLoading)
                {
                    await collection.LoadMoreItemsAsync(25);
                }
            }
        }

        private void CharacterImage_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if( e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse )
            {
                var properties = e.GetCurrentPoint(this).Properties;
                if( properties.IsLeftButtonPressed )
                {
                    NavArgs.RootFrame.Navigate(typeof(CharacterView), Convert.ToInt32((sender as ImageEx).Tag), 
                        new DrillInNavigationTransitionInfo());
                }
            }
        }
    }

    public class IncrementLoader: IncrementalLoadingCollection<CharacterListLoader, Edge>
    {
        private readonly int MediaId;
        private string StaffLang;
        private readonly CharacterListLoader Loader;

        public IncrementLoader(int mediaId, string staffLang, CharacterListLoader loader, string MediaType)
        {
            MediaId = mediaId;
            StaffLang = staffLang;
            Loader = loader;
            Loader.MediaId = MediaId;
            Loader.Langauge = StaffLang;
            Loader.MediaType = MediaType;
        }

        protected override Task<IEnumerable<Edge>> LoadDataAsync(CancellationToken cancellationToken)
        {
            return Loader.GetPagedItemsAsync(1, 1);
        }
    }

    public class CharacterListLoader : IIncrementalSource<Edge>
    {
        public int PageNum = 1;
        public bool HasNextPage = true;
        public int MediaId;
        public string Langauge;
        public string MediaType;
        public async Task<IEnumerable<Edge>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            List<Edge> result = new List<Edge>();
            if (HasNextPage)
            {
                var res = await Initialize.FetchData(MediaId, PageNum);
                if (MediaType == "ANIME" )
                {
                    foreach( var characterEdge in
                     from item in res.Data.Media.Characters.Edges
                     from VA in item.VoiceActorRoles
                     where VA.VoiceActor.LanguageV2 == Langauge
                     select new Edge
                     {
                         Role = item.Role,
                         SelectedVoiceActor = VA,
                         Node = item.Node,
                         MediaType = MediaType
                     } )
                    {
                        result.Add(characterEdge);
                    }
                } else
                {
                    foreach(var characterEdge in res.Data.Media.Characters.Edges )
                    {
                        result.Add(characterEdge);
                    }
                }
                HasNextPage = res.Data.Media.Characters.PageInfo.HasNextPage;
                PageNum++;
            }
            return result;
        }
    }
}
