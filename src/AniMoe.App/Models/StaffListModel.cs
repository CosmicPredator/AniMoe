using System;
using System.Collections.Generic;
using System.Globalization;
using AniMoe.App.Models.AnimeListModels;
using AniMoe.App.Services;
using AniMoe.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AniMoe.App.Models.StaffListModel
{
    public partial class StaffListModel
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("Media")]
        public Media Media { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("staff")]
        public Staff Staff { get; set; }
    }

    public partial class Staff
    {
        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("edges")]
        public List<Edge> Edges { get; set; }
    }

    public partial class Edge
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("node")]
        public Node Node { get; set; }
    }

    public partial class Node
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }
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
        [JsonProperty("hasNextPage")]
        public bool HasNextPage { get; set; }

        [JsonProperty("currentPage")]
        public long CurrentPage { get; set; }

        [JsonProperty("perPage")]
        public long PerPage { get; set; }
    }

    public static partial class Initialize
    {
        public static async Task<StaffListModel> FetchData(int mediaId, int pageNum = 1)
        {
            IRequestHandler Handler = App.Current.Services.GetService<IRequestHandler>();
            StaffListModel Model = await Handler.RequestApi<StaffListModel>(
                Queries.Query.StaffListQuery,
                new
                {
                    mediaId = mediaId,
                    pageNum = pageNum
                }
            );
            return Model;
        }
    }
}
