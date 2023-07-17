using AniMoe.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Models.CharacterModel
{
    public class CharacterModel
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("Character")]
        public Character Character { get; set; }
    }

    public partial class Character
    {
        [JsonProperty("isFavourite")]
        public bool IsFavourite { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("age")]
        public string Age { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("media")]
        public Media Media { get; set; }

        [JsonProperty("favourites")]
        public int Favourites { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("edges")]
        public Edge[] Edges { get; set; }
    }

    public partial class Edge
    {
        [JsonProperty("voiceActors")]
        public List<VoiceActor> VoiceActors { get; set; }

        [JsonProperty("node")]
        public Node Node { get; set; }
    }

    public partial class Node
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public Title Title { get; set; }

        [JsonProperty("coverImage")]
        public Image CoverImage { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("mediaListEntry")]
        public MediaListEntry MediaListEntry { get; set; }
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

    public partial class MediaListEntry
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class VoiceActor
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("languageV2")]
        public string LanguageV2 { get; set; }

        [JsonProperty("name")]
        public Title Name { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }
    }

    public partial class Name
    {
        [JsonProperty("userPreferred")]
        public string UserPreferred { get; set; }

        [JsonProperty("native")]
        public string Native { get; set; }
    }

    public static class Initialize
    {
        public static async Task<CharacterModel> FetchData(int characterId)
        {
            IRequestHandler Handler = App.Current.Services.GetRequiredService<IRequestHandler>();
            CharacterModel model = await Handler.RequestApi<CharacterModel>(
                Queries.Query.CharacterQuery,
                new
                {
                    id = characterId
                }
            );
            return model;
        }
    }
}
