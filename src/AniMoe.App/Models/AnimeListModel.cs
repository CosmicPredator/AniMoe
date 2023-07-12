using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AniMoe.App.Helpers;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using AniMoe.App.Services;
using Microsoft.Extensions.DependencyInjection;
using AniMoe.App.ViewModels;

namespace AniMoe.App.Models.AnimeListModels
{
    public partial class AnimeListModel
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

        [JsonProperty("nextAiringEpisode")]
        public NextAiringEpisode NextAiringEpisode { get; set; }

        [JsonProperty("episodes")]
        public int Episodes { get; set; }

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

    public partial class NextAiringEpisode
    {
        [JsonProperty("airingAt")]
        public int AiringAt { get; set; }

        [JsonProperty("timeUntilAiring")]
        public int TimeUntilAiring { get; set; }

        [JsonProperty("episode")]
        public int Episode { get; set; }
    }

    public partial class Title
    {
        [JsonProperty("userPreferred")]
        public string UserPreferred { get; set; }
    }

    public static partial class Initialize
    {
        public static async Task<AnimeListModel> FetchData()
        {
            IRequestHandler Handler = App.Current.Services.GetService<IRequestHandler>();
            MasterViewModel Vm = App.Current.Services.GetRequiredService<MasterViewModel>();
            AnimeListModel Model = await Handler.RequestApi<AnimeListModel>(
                Queries.Query.MediaListQuery,
                new
                {
                    userId = Vm.Model.Data.User.Id,
                    isAnime = true,
                    mediaType = "ANIME"
                }
            );
            return Model;
        }
    }
}
