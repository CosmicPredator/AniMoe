using AniMoe.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Models.ReviewExploreModel
{
    public partial class ReviewExploreModel
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

        [JsonProperty("reviews")]
        public List<Review> Reviews { get; set; }
    }

    public partial class PageInfo
    {
        [JsonProperty("hasNextPage")]
        public bool HasNextPage { get; set; }

        [JsonProperty("currentPage")]
        public long CurrentPage { get; set; }
    }

    public partial class Review
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("media")]
        public Media Media { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("rating")]
        public long Rating { get; set; }

        [JsonProperty("ratingAmount")]
        public long RatingAmount { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("bannerImage")]
        public Uri BannerImage { get; set; }

        [JsonProperty("title")]
        public Title Title { get; set; }
    }

    public partial class Title
    {
        [JsonProperty("userPreferred")]
        public string UserPreferred { get; set; }
    }

    public partial class User
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public static class Initialize
    {
        public static async Task<ReviewExploreModel> FetchData(int pageNum)
        {
            IRequestHandler Handler = App.Current.Services.GetRequiredService<IRequestHandler>();
            ReviewExploreModel model = await Handler.RequestApi<ReviewExploreModel>(
                Queries.Query.ReviewExploreQuery,
                new
                {
                    page = pageNum
                }
            );
            return model;
        }
    }
}
