using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AniMoe.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AniMoe.App.Models.MasterModel
{
    public partial class MasterModel
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("Viewer")]
        public User User { get; set; }

        [JsonProperty("MediaTagCollection")]
        public List<MediaTagCollection> MediaTagCollection { get; set; }

        [JsonProperty("GenreCollection")]
        public List<string> GenreCollection { get; set; }

        [JsonProperty("SourceList")]
        public FormatEnum MediaSourceList { get; set; }

        [JsonProperty("MediaSortEnum")]
        public FormatEnum MediaSortList { get; set; }

        [JsonProperty("MediaSeasonEnum")]
        public FormatEnum MediaSeasonList { get; set; }

        [JsonProperty("MediaFormatEnum")]
        public FormatEnum MediaFormatList { get; set; }
    }

    public partial class MediaTagCollection
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }

    public partial class User
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("avatar")]
        public Avatar Avatar { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("unreadNotificationCount")]
        public int UnreadNotificationCount { get; set; }
    }

    public partial class Avatar
    {
        [JsonProperty("large")]
        public Uri Large { get; set; }

        [JsonProperty("medium")]
        public Uri Medium { get; set; }
    }

    public partial class FormatEnum
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("enumValues")]
        public List<EnumValue> EnumValues { get; set; }
    }

    public partial class EnumValue
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public static class Initialize
    {
        public static async Task<MasterModel> FetchData()
        {
            IRequestHandler Handler = App.Current.Services.GetRequiredService<IRequestHandler>();
            MasterModel model = await Handler.RequestApi<MasterModel>(
                Queries.Query.MasterQuery
            );
            return model;
        }
    }
}
