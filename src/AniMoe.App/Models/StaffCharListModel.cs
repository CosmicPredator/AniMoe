using AniMoe.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Models.StaffCharListModel
{
    public partial class StaffCharListModel
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("Staff")]
        public Staff Staff { get; set; }
    }

    public partial class Staff
    {
        [JsonProperty("characterMedia")]
        public CharacterMedia CharacterMedia { get; set; }
    }

    public partial class CharacterMedia
    {
        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("edges")]
        public IEnumerable<Edge> Edges { get; set; }
    }

    public partial class Edge
    {
        [JsonProperty("characterRole")]
        public string CharacterRole { get; set; }

        [JsonProperty("characters")]
        public Character[] Characters { get; set; }

        [JsonProperty("node")]
        public Node Node { get; set; }

        [JsonIgnore]
        public Character SelectedCharacter { get; set; }
    }

    public partial class Character
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public Title Name { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("large")]
        public Uri Large { get; set; }
    }

    public partial class Title
    {
        [JsonProperty("userPreferred")]
        public string UserPreferred { get; set; }
    }

    public partial class Node
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public Title Title { get; set; }

        [JsonProperty("coverImage")]
        public Image CoverImage { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class PageInfo
    {
        [JsonProperty("hasNextPage")]
        public bool HasNextPage { get; set; }
    }

    public static partial class Initialize
    {
        public static async Task<StaffCharListModel> FetchData(int staffId, bool onList = false, int pageNum = 1)
        {
            IRequestHandler Handler = App.Current.Services.GetService<IRequestHandler>();
            StaffCharListModel Model = await Handler.RequestApi<StaffCharListModel>(
                Queries.Query.StaffCharQuery,
                new
                {
                    id = staffId,
                    onList,
                    page = pageNum
                }
            );
            return Model;
        }
    }
}
