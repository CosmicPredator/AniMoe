using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using AniMoe.App.Models;
using AniMoe.App.Helpers;
using AniMoe.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AniMoe.App.Dialogs
{
    public sealed partial class MediaListFliterView : ContentDialog
    {
        public MediaListFilterModel Model;
        private List<Genre> genreList;
        public List<Format> AnimeFormatList;
        public MasterViewModel DataModal = App.Current.Services
            .GetRequiredService<MasterViewModel>();
        public int Year = DateTime.Now.Year + 1;
        public MediaListFliterView()
        { 
            this.InitializeComponent();
            Model = new();
            ClearControls();
            InitLists();
        }

        private void InitLists()
        {
            AnimeFormatList = TagsGenres.FormatList.Where(
                x => x.Type == MediaType.ANIME || x.FormatEnumCode == null
            ).ToList();
            genreList = DataModal.Model.Data.GenreCollection.Select(
                x => new Genre
                {
                    Name = x,
                    IsChekced = false,
                }
            ).ToList();
        }

        public void LoadDataFromControl()
        {
            Model.Country = (Country)CountryComboBox.SelectedItem;
            Model.Status = (Status)StatusComboBox.SelectedItem;
            Model.Genres = genreList.Where(x => x.IsChekced == true).ToList().Count > 0 ? genreList.Where(x => x.IsChekced == true).Select(x => x.Name).ToList() : null;
            Model.Year = (int)YearBox.Value;
            Model.Format = (Format)FormatComboBox.SelectedItem;
        }

        private void ClearData()
        {
            InitLists();
            genreList.ForEach(x => x.IsChekced = false);
            GenreListView.ItemsSource = genreList;
            StatusComboBox.SelectedIndex = 0;
            CountryComboBox.SelectedIndex = 0;
            FormatComboBox.SelectedIndex = 0;
            YearBox.Value = 1950;
        }

        private void ClearControls()
        {
            Model.Genres = null;
            Model.Country = null;
            Model.Status = null;
            Model.Format = null;
            Model.Year = 1949;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            LoadDataFromControl();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ClearData();
            ClearControls();
        }
    }
}
