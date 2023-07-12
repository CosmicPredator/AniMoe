using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using AniMoe.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Labs.WinUI;
using CommunityToolkit.Mvvm.ComponentModel;
using Serilog;
using AniMoe.App.Models.MasterModel;
using CommunityToolkit.WinUI.UI.Controls.TextToolbarSymbols;
using AniMoe.App.Helpers;
using AniMoe.App.Queries;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace AniMoe.App.Dialogs
{
    public sealed partial class ExploreViewFilterDialog : ContentDialog
    {
        public MasterViewModel masterViewModel;
        public List<ExploreFilterModel> FilterList;
        public Dictionary<string, dynamic> Filters = new();
        public List<EnumValue> FormatList;
        public string MediaType;

        public ExploreViewFilterDialog(string mediaType)
        {
            masterViewModel = App.Current.Services.GetRequiredService<MasterViewModel>();
            MediaType = mediaType;
            InitializeComponent();
            LoadFilterList();
            LoadUI();
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            GC.Collect();
        }

        private void TagGenrePivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Segmented seg = sender as Segmented;
            TagGenreListView.ItemsSource = (seg.SelectedValue as ExploreFilterModel).Values;
        }

        private void LoadUI()
        {
            if (MediaType == "ANIME" )
            {
                FormatList = masterViewModel.Model.Data.MediaFormatList.EnumValues;
                EpisodeChapterRangeSelector.Maximum = 150;
                DurationVolumeRangeSelector.Maximum = 170;
            } else
            {
                List<EnumValue> slicedList = new();
                slicedList.Add(masterViewModel.Model.Data.MediaFormatList.EnumValues[0]);
                slicedList.AddRange(masterViewModel.Model.Data.MediaFormatList.EnumValues.TakeLast(3));
                FormatList = slicedList;
                EpisodeChapterRangeSelector.Maximum = 500;
                DurationVolumeRangeSelector.Maximum = 50;
            }
        }

        private void ResetTagsGenres()
        {
            FilterList.ForEach(
                 category => category.Values.ForEach(
                     value => value.IsChecked = "false"
                 )
            );
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ResetTagsGenres();
        }

        private void LoadFilterList()
        {
            FilterList = new();

            // Loading Genre Lists
            FilterList.Add(new ExploreFilterModel
            {
                CategoryName = "Genres",
                Values = masterViewModel.Model.Data.GenreCollection.Select(
                    x => new TagGenreObject
                    {
                        Name = x,
                        IsChecked = "false",
                        Description = string.Empty
                    }
                ).ToList()
            });

            // Loading other Tags List
            var GroupedTag = masterViewModel.Model.Data.MediaTagCollection.GroupBy(
                x => x.Category
            );
            FilterList.AddRange(GroupedTag.Select(
                x => new ExploreFilterModel
                {
                    CategoryName = x.Key,
                    Values = x.Select(
                        y => new TagGenreObject
                        {
                            Name = y.Name,
                            Description = y.Description,
                            IsChecked = "false"
                        }
                    ).ToList()
                }
            ).ToList());
        }

        public void InflateFilterObject()
        {
            Filters = new();
            Filters["sortOptions"] = new string[] { (SortComboBox.SelectedValue as EnumValue).Name };
            if( (AiringStatusTextBox.SelectedValue as Status).StatusEnumCode != null )
                Filters["airingStatus"] = (AiringStatusTextBox.SelectedValue as Status).StatusEnumCode;
            if( (SeasonComboBox.SelectedValue as EnumValue).Name != "Select One" )
                Filters["season"] = (SeasonComboBox.SelectedValue as EnumValue).Name;
            if( (FormatComboBox.SelectedValue as EnumValue).Name != "Select One" )
                Filters["format"] = (FormatComboBox.SelectedValue as EnumValue).Name;
            if( (CountryComboBox.SelectedValue as Country).CountryCode != null )
                Filters["country"] = (CountryComboBox.SelectedValue as Country).CountryCode;
            if( (SourceComboBox.SelectedValue as EnumValue).Name != "Select One" )
                Filters["sourceMaterial"] = (SourceComboBox.SelectedValue as EnumValue).Name;
            if (MediaType == "ANIME" )
            {
                if (EpisodeChapterRangeSelector.RangeStart != 0 || EpisodeChapterRangeSelector.RangeEnd != 150 )
                {
                    Filters["episodesStartRange"] = (int)EpisodeChapterRangeSelector.RangeStart;
                    Filters["episodesEndRange"] = (int)EpisodeChapterRangeSelector.RangeEnd + 1;
                }

                if (DurationVolumeRangeSelector.RangeStart != 0 || DurationVolumeRangeSelector.RangeEnd != 170 )
                {
                    Filters["durationStartRange"] = (int)DurationVolumeRangeSelector.RangeStart;
                    Filters["durationEndRange"] = (int)DurationVolumeRangeSelector.RangeEnd + 1;
                }
            } else if (MediaType == "MANGA" )
            {
                if( EpisodeChapterRangeSelector.RangeStart != 0 || EpisodeChapterRangeSelector.RangeEnd != 500 )
                {
                    Filters["chapterStartRange"] = (int)EpisodeChapterRangeSelector.RangeStart;
                    Filters["chapterEndRange"] = (int)EpisodeChapterRangeSelector.RangeEnd + 1;
                }

                if (DurationVolumeRangeSelector.Minimum != 0 || DurationVolumeRangeSelector.RangeEnd != 50 )
                {
                    Filters["volumeStartRange"] = (int)DurationVolumeRangeSelector.RangeStart;
                    Filters["volumeEndRange"] = (int)DurationVolumeRangeSelector.RangeEnd + 1;
                }
            }
            if( YearRangeSelector.RangeStart != 1970 || YearRangeSelector.RangeEnd != 2024 )
            {
                Filters["yearStartRange"] = $"{YearRangeSelector.RangeStart - 1}9999";
                Filters["yearEndRange"] = $"{YearRangeSelector.RangeEnd + 1}0000";
            }
            if( (bool)DoujinCheckBox.IsChecked ) Filters["isDoujin"] = DoujinCheckBox.IsChecked;
            if( (bool)AdultCheckBox.IsChecked ) Filters["isAdult"] = AdultCheckBox.IsChecked;
            if( ShowListCheckBox.IsEnabled && (bool)ShowListCheckBox.IsChecked )
                Filters["onList"] = true;
            if( HideListCheckBox.IsEnabled && (bool)HideListCheckBox.IsChecked )
                Filters["onList"] = false;
            List<TagGenreObject> GenreInList = FilterList.Where(x => x.CategoryName == "Genres").First().Values.Where(
                x => x.IsChecked.ToLower() == "true"
             ).ToList();
            if (GenreInList.Count > 0) Filters["genresIn"] = GenreInList.Select(x => x.Name).ToList();
            List<TagGenreObject> GenreNotInList = FilterList.Where(x => x.CategoryName == "Genres").First().Values.Where(
                x => x.IsChecked == "intermediate"
            ).ToList();
            if (GenreNotInList.Count > 0 ) Filters["genresNotIn"] = GenreNotInList.Select(x => x.Name).ToList();
            List<TagGenreObject> TagsInList = FilterList
                .Where(i => i.CategoryName != "Genres")
                .SelectMany(i => i.Values.Where(j => j.IsChecked.ToLower() == "true")).ToList();
            if( TagsInList.Count > 0 ) Filters["tagsIn"] = TagsInList.Select(x => x.Name).ToList();
            List<TagGenreObject> TagsNotInList = FilterList
                .Where(i => i.CategoryName != "Genres")
                .SelectMany(i => i.Values.Where(j => j.IsChecked.ToLower() == "intermediate")).ToList();
            if( TagsNotInList.Count > 0 ) Filters["tagsNotIn"] = TagsNotInList.Select(x => x.Name).ToList();
        }

        private void ContentDialog_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            InflateFilterObject();
            var jsonString = new StringContent(
                JsonConvert.SerializeObject(
                    Filters
                ),
                Encoding.UTF8,
                "application/json"
            );
            Log.Information(await jsonString.ReadAsStringAsync());
        }
    }

    public class ExploreFilterModel : ObservableObject
    {
        private string categoryName;
        public string CategoryName
        {
            get => categoryName;
            set => SetProperty(ref categoryName, value);
        }
        public List<TagGenreObject> Values { get; set; }
    }

    public class TagGenreObject : ObservableObject
    {
        private string name;
        private string isChecked;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public string IsChecked
        {
            get => isChecked;
            set => SetProperty(ref isChecked, value);
        }
        public string Description { get; set; }
    }
}
