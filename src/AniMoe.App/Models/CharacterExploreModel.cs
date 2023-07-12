using AniMoe.App.Models.AnimeListModels;
using AniMoe.App.Services;
using AniMoe.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Models.CharacterExploreModel
{
    public partial class CharacterExploreModel
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

        [JsonProperty("characters")]
        public List<Character> Characters { get; set; }
    }

    public partial class Character
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("large")]
        public Uri Large { get; set; }
    }

    public partial class Name
    {
        [JsonProperty("userPreferred")]
        public string UserPreferred { get; set; }
    }

    public partial class PageInfo
    {
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }

        [JsonProperty("hasNextPage")]
        public bool HasNextPage { get; set; }
    }

    public static class Initialize
    {
        public static async Task<CharacterExploreModel> FetchData(Dictionary<string, dynamic> Filters)
        {
            IRequestHandler Handler = App.Current.Services.GetRequiredService<IRequestHandler>();
            CharacterExploreModel model = await Handler.RequestApi<CharacterExploreModel>(
                Queries.Query.CharacterListExploreQuery,
                Filters
            );
            return model;
        }
    }
}
