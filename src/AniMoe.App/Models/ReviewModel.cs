using AniMoe.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Models.ReviewModel
{
    public partial class ReviewModel
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("Review")]
        public Review Review { get; set; }
    }

    public partial class Review
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("media")]
        public Media Media { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("rating")]
        public long Rating { get; set; }

        [JsonProperty("ratingAmount")]
        public long RatingAmount { get; set; }

        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }
    }

    public partial class Media
    {
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
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public static class Initialize
    {
        public static async Task<ReviewModel> FetchData(int ReviewId)
        {
            IRequestHandler Handler = App.Current.Services.GetRequiredService<IRequestHandler>();
            ReviewModel model = await Handler.RequestApi<ReviewModel>(
                Queries.Query.ReviewQuery,
                new
                {
                    id = ReviewId
                }
            );
            return model;
        }
    }
}
