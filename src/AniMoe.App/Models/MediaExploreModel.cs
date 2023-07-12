using System;
using System.Collections.Generic;
using System.Globalization;
using AniMoe.App.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using AniMoe.App.Models.AnimeListModels;

namespace AniMoe.App.Models.MediaExploreModel
{
    public partial class MediaExploreModel
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("Page")]
        public Page Page { get; set; }
    }

    public partial class Page
    {
        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("AnimeManga")]
        public List<AnimeManga> AnimeManga { get; set; }
    }

    public partial class AnimeManga
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coverImage")]
        public CoverImage CoverImage { get; set; }

        [JsonProperty("title")]
        public Title Title { get; set; }
    }

    public partial class CoverImage
    {
        [JsonProperty("large")]
        public Uri Large { get; set; }
    }

    public partial class Title
    {
        [JsonProperty("userPreferred")]
        public string UserPreferred { get; set; }
    }

    public partial class PageInfo
    {
        [JsonProperty("perPage")]
        public int PerPage { get; set; }

        [JsonProperty("hasNextPage")]
        public bool HasNextPage { get; set; }

        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }
    }

    public static class Initialize
    {
        public static async Task<MediaExploreModel> FetchData(Dictionary<string, dynamic> Filters)
        {
            IRequestHandler Handler = App.Current.Services.GetRequiredService<IRequestHandler>();
            MediaExploreModel model = await Handler.RequestApi<MediaExploreModel>(
                Queries.Query.MediaExploreQuery,
                Filters
            );
            return model;
        }
    }
}
