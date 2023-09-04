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
    public partial record CharacterExploreModel
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial record Data
    {
        [JsonProperty("Character")]
        public Page CharacterList { get; set; }

        [JsonProperty("Staff")]
        public Page StaffList { get; set; }
    }

    public partial record Page
    {
        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("characters")]
        public List<Character> Characters { get; set; }

        [JsonProperty("staff")]
        public List<Character> Staffs { get; set; }
    }

    public partial record Character
    {
        [JsonProperty("__typename")]
        public string TypeName { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }
    }

    public partial record Image
    {
        [JsonProperty("large")]
        public Uri Large { get; set; }
    }

    public partial record Name
    {
        [JsonProperty("userPreferred")]
        public string UserPreferred { get; set; }
    }

    public partial record PageInfo
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
                Queries.Query.CharacterStaffListExploreQuery,
                Filters
            );
            return model;
        }
    }
}
