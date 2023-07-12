using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AniMoe.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AniMoe.App.Models.MediaListStatusModel
{
    public partial class MediaListStatusModel
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }   

    public partial class Data
    {
        [JsonProperty("AnimeStatusList")]
        public StatusList AnimeStatusList { get; set; }

        [JsonProperty("MangaStatusList")]
        public StatusList MangaStatusList { get; set; }
    }

    public partial class StatusList
    {
        [JsonProperty("lists")]
        public List<Lists> Lists { get; set; }
    }

    public partial class Lists
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isCustomList")]
        public bool IsCustomList { get; set; }
    }

    public static partial class Initialize
    {
        public static async Task<MediaListStatusModel> FetchData(int userId)
        {
            IRequestHandler Handler = App.Current.Services.GetService<IRequestHandler>();
            MediaListStatusModel Model = await Handler.RequestApi<MediaListStatusModel>(
                Queries.Query.StatusListQuery,
                new
                {
                    userId = userId
                }
            );
            return Model;
        }
    }
}
