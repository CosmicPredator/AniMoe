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

namespace AniMoe.App.Models.UserActivityModel
{
    public partial class UserActivityModel
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
        [JsonProperty("activities")]
        public ObservableCollection<Activity> Activities { get; set; }
    }

    public partial class Activity
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public User User { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("replyCount")]
        public long ReplyCount { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("progress")]
        public string Progress { get; set; }

        [JsonProperty("isSubscribed")]
        public bool IsSubscribed { get; set; }

        [JsonProperty("likeCount")]
        public long LikeCount { get; set; }

        [JsonProperty("isLiked")]
        public bool IsLiked { get; set; }

        [JsonProperty("isPinned", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPinned { get; set; }

        [JsonProperty("siteUrl")]
        public Uri SiteUrl { get; set; }

        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        [JsonProperty("media", NullValueHandling = NullValueHandling.Ignore)]
        public Media Media { get; set; }

        [JsonProperty("recipientId", NullValueHandling = NullValueHandling.Ignore)]
        public long? RecipientId { get; set; }

        [JsonProperty("recipient", NullValueHandling = NullValueHandling.Ignore)]
        public Messenger Recipient { get; set; }

        [JsonProperty("messengerId", NullValueHandling = NullValueHandling.Ignore)]
        public long? MessengerId { get; set; }

        [JsonProperty("messenger", NullValueHandling = NullValueHandling.Ignore)]
        public Messenger Messenger { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("isPrivate", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPrivate { get; set; }

        [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
        public long? UserId { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public Title Title { get; set; }

        [JsonProperty("coverImage")]
        public CoverImage CoverImage { get; set; }
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

    public partial class Messenger
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("avatar")]
        public Avatar Avatar { get; set; }
    }

    public partial class Avatar
    {
        [JsonProperty("medium")]
        public Uri Medium { get; set; }
    }

    public partial class User
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("avatar")]
        public Avatar Avatar { get; set; }
    }

    public static partial class Initialize
    {
        public static async Task<UserActivityModel> FetchData(int userId)
        {
            IRequestHandler Handler = App.Current.Services.GetService<IRequestHandler>();
            UserActivityModel Model = await Handler.RequestApi<UserActivityModel>(
                Queries.Query.UserActivityQuery,
                new
                {
                    userId
                }
            );
            return Model;
        }
    }
}
