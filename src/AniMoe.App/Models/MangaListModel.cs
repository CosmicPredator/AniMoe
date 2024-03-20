using AniMoe.App.Services;
using AniMoe.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Models.MangaListModel
{
    public partial class MangaListModel
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("MediaListCollection")]
        public MediaListCollection MediaListCollection { get; set; }
    }

    public partial class MediaListCollection
    {

        [JsonProperty("lists")]
        public List<ListObj> Lists { get; set; }
    }

    public partial class ListObj
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isCustomList")]
        public bool IsCustomList { get; set; }

        [JsonProperty("entries")]
        public ObservableCollection<Entry> Entries { get; set; }
    }

    public partial class Entry
    {
        [JsonProperty("progress")]
        public int Progress { get; set; }

        [JsonProperty("media")]
        public Media Media { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public Title Title { get; set; }

        [JsonProperty("coverImage")]
        public CoverImage CoverImage { get; set; }

        [JsonProperty("chapters")]
        public int Chapters { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("genres")]
        public List<string> Genres { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("countryOfOrigin")]
        public string CountryOfOrigin { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }

        [JsonProperty("seasonYear")]
        public int SeasonYear { get; set; }

        [JsonProperty("siteUrl")]
        public string SiteUrl { get; set; }
    }

    public partial class Tag
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class CoverImage
    {
        [JsonProperty("large")]
        public Uri Large { get; set; }

        [JsonProperty("extraLarge")]
        public Uri ExtraLarge { get; set; }
    }

    public partial class Title
    {
        [JsonProperty("userPreferred")]
        public string UserPreferred { get; set; }
    }

    public static partial class Initialize
    {
        public static async Task<MangaListModel> FetchData()
        {
            IRequestHandler Handler = App.Current.Services.GetService<IRequestHandler>();
            SplashViewModel Vm = App.Current.Services.GetRequiredService<SplashViewModel>();
            MangaListModel Model = await Handler.RequestApi<MangaListModel>(
                Queries.Query.MediaListQuery,
                new
                {
                    userId = Vm.Model.Data.User.Id,
                    isAnime = false,
                    mediaType = "MANGA"
                }
            );
            return Model;
        }
    }
}