using AniMoe.App.Dialogs;
using AniMoe.App.Helpers;
using AniMoe.App.Models.CharacterExploreModel;
using AniMoe.App.Models.MediaExploreModel;
using CommunityToolkit.Common.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AniMoe.App.ViewModels
{
    public class ExploreViewModel : ObservableRecipient
    {
        private ExploreViewFilterDialog filterDialog;
        private Dictionary<string, dynamic> filterObject;
        public MediaIncrementalList MediaListCollection;
        public CharacterIncrementalList CharacterListCollection;
        private ItemsRepeater MasterItemsRepeater;
        private ScrollViewer ItScrollViewer;
        private DispatcherQueue dispatcherQueue;
        private string selectedType = "Anime";
        private DispatcherTimer searchTextProcessingTimer;
        private TextBox SearchBox;
        private bool isLoading;
        private bool isFilterButtonEnabled = true;
        private bool isBirthdayButtonEnabled = false;
        private bool isBirthdayEnabled;

        public string SelectedType
        {
            get => selectedType;
            set => SetProperty(ref selectedType, value);
        }

        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public bool IsFilterButtonEnabled
        {
            get => isFilterButtonEnabled;
            set => SetProperty(ref isFilterButtonEnabled, value);
        }

        public bool IsBirthdayButtonEnabled
        {
            get => isBirthdayButtonEnabled;
            set => SetProperty(ref isBirthdayButtonEnabled, value);
        }

        public bool IsBirthdayEnabled
        {
            get => isBirthdayEnabled;
            set => SetProperty(ref isBirthdayEnabled, value);
        }

        public List<string> ExploreTypes = new()
        {
            "Anime",
            "Manga",
            "Character",
            "Staff",
            "Users",
            "Reviews"
        };

        public ExploreViewModel(DispatcherQueue queue)
        {
            dispatcherQueue = queue;
            searchTextProcessingTimer = new DispatcherTimer();
            searchTextProcessingTimer.Interval = TimeSpan.FromMilliseconds(500);
            searchTextProcessingTimer.Tick += TextProcessingTimer_Tick;
        }

        private void TextProcessingTimer_Tick(object sender, object e)
        {
            searchTextProcessingTimer.Stop();
            
            if( SearchBox.Text == "" )
            {
                filterDialog = new(SelectedType);
                filterObject.Remove("search");
            }
            else
            {
                filterDialog = new(SelectedType);
                filterObject["search"] = SearchBox.Text;
            }
                
            if( SelectedType.ToUpper() == "ANIME" || SelectedType.ToUpper() == "MANGA" )
            {
                InitMediaListView(SelectedType.ToUpper());
            }

            if ( SelectedType.ToUpper() == "CHARACTER" )
            {
                InitCharacterListView(IsBirthdayEnabled, SearchBox.Text);
            }
        }

        public void SearchBox_TextChanged(object sender, TextChangedEventArgs args)
        {
            searchTextProcessingTimer.Stop();
            searchTextProcessingTimer.Start();
        }

        public async void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            filterDialog.XamlRoot = btn.XamlRoot;
            ContentDialogResult result = await filterDialog.ShowAsync();
            if (result == ContentDialogResult.Primary )
            {
                filterObject = filterDialog.Filters;
                InitMediaListView(SelectedType.ToUpper());
            } else if (result == ContentDialogResult.Secondary )
            {
                filterDialog = new(SelectedType.ToUpper());
                filterObject = filterDialog.Filters;
                InitMediaListView(SelectedType.ToUpper());
            }
        }

        public void PassControls(ItemsRepeater itemsRepeater, 
            ScrollViewer scrollViewer, TextBox searchBox)
        {
            MasterItemsRepeater = itemsRepeater;
            ItScrollViewer = scrollViewer;
            SearchBox = searchBox;
        }

        private void InitMediaListView(string MediaType)
        {
            MediaListCollection = null;
            CharacterListCollection = null;
            IsLoading = true;
            if( MediaListCollection == null )
            {
                MediaListCollection = new MediaIncrementalList(filterObject, MediaType, new MediaIncrementLoader());
                dispatcherQueue.TryEnqueue(() =>
                {
                    MasterItemsRepeater.ItemsSource = MediaListCollection;
                });
                if( ItScrollViewer != null )
                {
                    ItScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
                }
                Task.Run(async () =>
                {
                    await dispatcherQueue.EnqueueAsync(async () =>
                    {
                        await MediaListCollection.LoadMoreItemsAsync(25);
                        IsLoading = false;
                    });
                });
            } 
        }

        private void InitCharacterListView(bool isBirthDay, string search = "")
        {
            MediaListCollection = null;
            CharacterListCollection = null;
            IsLoading = true;
            if( CharacterListCollection == null )
            {
                CharacterListCollection = new CharacterIncrementalList(new CharacterIncrementalLoader(), false, search);
                dispatcherQueue.TryEnqueue(() =>
                {
                    MasterItemsRepeater.ItemsSource = CharacterListCollection;
                });
                if( ItScrollViewer != null )
                {
                    ItScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
                }
                Task.Run(async () =>
                {
                    await dispatcherQueue.EnqueueAsync(async () =>
                    {
                        await CharacterListCollection.LoadMoreItemsAsync(25);
                        IsLoading = false;
                    });
                });
            }

        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if( scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 20.0 )
            {
                if( SelectedType.ToUpper() == "ANIME" || SelectedType.ToUpper() == "MANGA" )
                {
                    if( !MediaListCollection.IsLoading )
                    {
                        await MediaListCollection.LoadMoreItemsAsync(25);
                    }
                }

                if( SelectedType.ToUpper() == "CHARACTER" )
                {
                    if( !CharacterListCollection.IsLoading )
                    {
                        await CharacterListCollection.LoadMoreItemsAsync(25);
                    }
                }
            }
        }

        public void MediaTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filterDialog = new(SelectedType.ToUpper());
            filterObject = filterDialog.Filters;
            if( SelectedType.ToUpper() == "ANIME" || SelectedType.ToUpper() == "MANGA" )
            {
                IsFilterButtonEnabled = true;
                IsBirthdayButtonEnabled = !IsFilterButtonEnabled;
                InitMediaListView(SelectedType.ToUpper());
            }
                
            if (SelectedType.ToUpper() == "CHARACTER" )
            {
                IsFilterButtonEnabled = false;
                IsBirthdayButtonEnabled = !IsFilterButtonEnabled;
                InitCharacterListView(IsBirthdayEnabled);
            }
                
        }

        public void BDayToggleButton_Click(object sender, RoutedEventArgs e)
        {
            InitCharacterListView(IsBirthdayEnabled);
        }
    }

    public class MediaIncrementalList : IncrementalLoadingCollection<MediaIncrementLoader, AnimeManga>
    {
        public MediaIncrementLoader Loader;
        public MediaIncrementalList(Dictionary<string, dynamic> filters, 
            string mediaType, MediaIncrementLoader loader)
        {
            Loader = loader;
            Loader.Filters = filters;
            Loader.MediaType = mediaType;
        }

        protected override Task<IEnumerable<AnimeManga>> LoadDataAsync(CancellationToken cancellationToken)
        {
            return Loader.GetPagedItemsAsync(1, 1);
        }
    }

    public class MediaIncrementLoader : IIncrementalSource<AnimeManga>
    {
        public bool HasNextPage = true;
        public int CurrentPage = 1;
        public Dictionary<string, dynamic> Filters;
        public string MediaType;
        public async Task<IEnumerable<AnimeManga>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            Filters["page"] = CurrentPage;
            Filters["type"] = MediaType.ToUpper();
            if( HasNextPage )
            {
                MediaExploreModel res = await Models.MediaExploreModel.Initialize.FetchData(Filters);
                HasNextPage = res.Data.Page.PageInfo.HasNextPage;
                CurrentPage++;
                return res.Data.Page.AnimeManga;
            }
            return new List<AnimeManga>();
        }
    }


    public class CharacterIncrementalList : IncrementalLoadingCollection<CharacterIncrementalLoader, Character>
    {
        public CharacterIncrementalLoader Loader;
        public CharacterIncrementalList(CharacterIncrementalLoader loader, bool isBirthday, string search = "")
        {
            Loader = loader;
            Loader.IsBirthday = isBirthday;
            Loader.Search = search;
        }

        protected override Task<IEnumerable<Character>> LoadDataAsync(CancellationToken cancellationToken)
        {
            return Loader.GetPagedItemsAsync(1, 1);
        }
    }

    public class CharacterIncrementalLoader : IIncrementalSource<Character>
    {
        public bool HasNextPage = true;
        public int CurrentPage = 1;
        public bool IsBirthday;
        public string Search;
        public Dictionary<string, dynamic> Filters = new();

        public async Task<IEnumerable<Character>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            if( HasNextPage )
            {
                Filters["page"] = CurrentPage;
                if (IsBirthday) Filters["isBirthday"] = IsBirthday;
                if( !string.IsNullOrEmpty(Search) ) Filters["search"] = Search;
                CharacterExploreModel res = await Models.CharacterExploreModel.Initialize.FetchData(Filters);
                HasNextPage = res.Data.Page.PageInfo.HasNextPage;
                CurrentPage++;
                return res.Data.Page.Characters;
            }
            return new List<Character>();
        }
    }
}
