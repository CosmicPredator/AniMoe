using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AniMoe.App.Models.AnimeListModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using AniMoe.App.Dialogs;
using AniMoe.App.Models;
using AniMoe.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Devices.PointOfService;
using Windows.UI;
using Microsoft.UI.Xaml.Media;
using System.ComponentModel.Design;

namespace AniMoe.App.ViewModels
{
    public partial class AnimeListViewModel : ObservableObject
    {
        private AnimeListModel model;
        private SplashViewModel StatusModel;
        private bool loaded = false;
        private bool isLoading = true;
        private bool isEmpty = false;
        private ObservableCollection<string> statusLists;
        private ObservableCollection<Entry> currentListView;
        private string selectedStatus;
        private MediaListFliterView filterDialog;
        private bool emptyCollection = false;
        private Brush buttonColor = (Brush)App.Current.Resources["CardBackgroundFillColorDefaultBrush"];

        [ObservableProperty]
        private bool loadStatusBar;

        public AnimeListModel Model
        {
            get => model;
            set => SetProperty(ref model, value);
        }

        public bool Loaded
        {
            get => loaded;
            set => SetProperty(ref loaded, value);
        }

        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public bool IsEmpty
        {
            get => isEmpty;
            set => SetProperty(ref isEmpty, value);
        }

        public Brush ButtonColor
        {
            get => buttonColor;
            set => SetProperty(ref buttonColor, value);
        }

        public bool EmptyCollection
        {
            get => emptyCollection;
            set => SetProperty(ref emptyCollection, value);
        }

        public IAsyncRelayCommand LoadView { get; }
        public IAsyncRelayCommand ReloadCollection { get; }

        public ObservableCollection<string> StatusLists
        {
            get => statusLists;
            set => SetProperty(ref statusLists, value);
        }

        public string SelectedStatus
        {
            get => selectedStatus;
            set => SetProperty(ref selectedStatus, value);
        }

        public ObservableCollection<Entry> CurrentListView
        {
            get => currentListView;
            set => SetProperty(ref currentListView, value);
        }

        public void StatusChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if( CurrentListView != null && Model != null && StatusLists != null)
            {
                Loaded = false;
                CurrentListView = Model.Data.MediaListCollection.Lists.Where(
                    x => x.Name == SelectedStatus
                ).First().Entries;
                ApplyFilters(filterDialog.Model);
                Loaded = true;
            }
        }

        public void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (!string.IsNullOrEmpty(box.Text))
            {
                Loaded = false;
                ApplyFilters(filterDialog.Model);
                CurrentListView = new ObservableCollection<Entry>(Model.Data.MediaListCollection.Lists.Where(
                    x => x.Name == SelectedStatus
                ).First().Entries.Where(
                    y => y.Media.Title.UserPreferred.ToLower().Contains(box.Text)
                ));
                Loaded = true;
            }
            else
            {
                Loaded = false;
                ApplyFilters(filterDialog.Model);
                CurrentListView = new ObservableCollection<Entry>(
                    Model.Data.MediaListCollection.Lists.Where(
                    x => x.Name == SelectedStatus
                ).First().Entries);
                Loaded = true;
            }
        }

        public AnimeListViewModel(SplashViewModel splashViewModel)
        {
            StatusModel = splashViewModel;
            LoadView = new AsyncRelayCommand(async() =>
            {
                await InitView();
            });
            ReloadCollection = new AsyncRelayCommand(reloadCollection);
            filterDialog = new();
        }

        public async Task InitView(bool isReload = false)
        {
            if( !isReload )
            {
                Loaded = false;
                IsLoading = !Loaded;
                LoadStatusBar = false;
            }
            if( StatusLists == null )
            {
                StatusLists = new(
                    StatusModel.mediaListStatusModel.Data.AnimeStatusList
                        .Lists.Select(x => x.Name).ToList()
                );
            }
            if ( Model == null || isReload)
            {
                Model = await Initialize.FetchData();
            }
            LoadStatusBar = true;
            if( Model.Data.MediaListCollection.Lists.Count > 0 )
            {
                CurrentListView = Model.Data.MediaListCollection.Lists.Where(
                    x => x.Name == SelectedStatus
                ).FirstOrDefault().Entries;
                ApplyFilters(filterDialog.Model);
                IsEmpty = false;
            }
            else
            {
                CurrentListView = null;
                IsEmpty = true;
            }
            Loaded = true;
            IsLoading = !Loaded;
        }

        public async void FilterButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var btn = sender as Button;
            filterDialog.XamlRoot = btn.XamlRoot;
            var result = await filterDialog.ShowAsync();
            if( result == ContentDialogResult.Primary)
            {
                ApplyFilters(filterDialog.Model);
            } else if ( result == ContentDialogResult.Secondary )
            {
                ApplyFilters(filterDialog.Model);
            }
        }

        private async Task reloadCollection()
        {
            Loaded = false;
            IsLoading = !Loaded;
            LoadStatusBar = false;
            StatusLists = null;
            await StatusModel.LoadFromApi();
            await InitView(true);
        }

        private void ApplyFilters(MediaListFilterModel filter)
        {
            IEnumerable<Entry> filteredList = Model.Data.MediaListCollection.Lists.Where(
                x => x.Name == SelectedStatus
            ).First().Entries;
            if( CurrentListView != null )
            {
                if( filter.Genres == null &&
                    filter.Status == null &&
                    filter.Format == null &&
                    filter.Country == null &&
                    filter.Year < 1950 )
                {
                    Loaded = false;
                    IsLoading = !Loaded;
                    CurrentListView = new ObservableCollection<Entry>(filteredList);
                    Loaded = true;
                    IsLoading = !Loaded;
                    ButtonColor = (Brush)App.Current.Resources["CardBackgroundFillColorDefaultBrush"];

                }
                else
                {
                    if( filter.Status.StatusEnumCode == null &&
                        filter.Format.FormatEnumCode == null &&
                        filter.Country.CountryCode == null &&
                        filter.Genres == null &&
                        filter.Year <= 1950 )
                    {
                        Loaded = false;
                        IsLoading = !Loaded;
                        CurrentListView = new ObservableCollection<Entry>(filteredList);
                        Loaded = true;
                        IsLoading = !Loaded;
                        ButtonColor = (Brush)App.Current.Resources["CardBackgroundFillColorDefaultBrush"];
                    }
                    else
                    {
                        Loaded = false;
                        IsLoading = !Loaded;
                        if( filter.Genres != null && filter.Genres.Count > 0 )
                        {
                            filteredList = filteredList.Where(
                                model => filter.Genres.All(
                                    element => model.Media.Genres.Contains(element)
                                )
                            );

                        }

                        if( filter.Status != null && filter.Status.StatusEnumCode != null )
                        {
                            filteredList = filteredList.Where(
                                x => x.Media.Status == filter.Status.StatusEnumCode
                            );

                        }

                        if( filter.Format != null && filter.Format.FormatEnumCode != null )
                        {
                            filteredList = filteredList.Where(
                                x => x.Media.Format == filter.Format.FormatEnumCode
                            );
                        }

                        if( filter.Country != null && filter.Country.CountryCode != null )
                        {
                            filteredList = filteredList.Where(
                                x => x.Media.CountryOfOrigin == filter.Country.CountryCode
                            );
                        }

                        if( filter.Year != null && filter.Year > 1950 && filter.Year <= DateTime.Now.Year )
                        {
                            filteredList = filteredList.Where(
                                x => x.Media.SeasonYear == filter.Year
                            );
                        }
                        CurrentListView = new ObservableCollection<Entry>(filteredList);
                        ButtonColor = new SolidColorBrush((Color)App.Current.Resources["SystemAccentColor"]);
                        Loaded = true;
                        IsLoading = !Loaded;
                    }
                }
            }
        }
    }
}
