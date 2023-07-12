using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using AniMoe.App.Models.AnimeListModels;
using AniMoe.App.Services;
using AniMoe.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AniMoe.App.Models.CharacterListModel
{
    public partial class CharacterListModel
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
        [JsonProperty("characters")]
        public Characters Characters { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Characters
    {
        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("edges")]
        public ObservableCollection<Edge> Edges { get; set; }
    }

    public partial class Edge
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonIgnore]
        public string MediaType { get; set; }

        [JsonProperty("voiceActorRoles")]
        public List<VoiceActorRole> VoiceActorRoles { get; set; }

        [JsonIgnore]
        public VoiceActorRole SelectedVoiceActor { get; set; }

        [JsonProperty("node")]
        public Node Node { get; set; }
    }

    public partial class Node
    {
        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }

    public partial class Name
    {
        [JsonProperty("userPreferred")]
        public string UserPreferred { get; set; }
    }

    public partial class VoiceActorRole
    {
        [JsonProperty("roleNotes")]
        public string RoleNotes { get; set; }

        [JsonProperty("voiceActor")]
        public VoiceActor VoiceActor { get; set; }
    }

    public partial class VoiceActor
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("languageV2")]
        public string LanguageV2 { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("large")]
        public Uri Large { get; set; }
    }

    public partial class PageInfo
    {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("perPage")]
        public long PerPage { get; set; }

        [JsonProperty("hasNextPage")]
        public bool HasNextPage { get; set; }

        [JsonProperty("currentPage")]
        public long CurrentPage { get; set; }
    }

    public static partial class Initialize
    {
        public static async Task<CharacterListModel> FetchData(int mediaId, int pageNum = 1)
        {
            IRequestHandler Handler = App.Current.Services.GetService<IRequestHandler>();
            CharacterListModel Model = await Handler.RequestApi<CharacterListModel>(
                Queries.Query.CharacterListQuery,
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
