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
using System.Diagnostics;

namespace AniMoe.App.Models.MediaModel
{
    public partial class MediaModel
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("Media")]
        public Media Media { get; set; }

        [JsonProperty("Page")]
        public Pagination Page { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("nextAiringEpisode")]
        public NextAiringEpisode NextAiringEpisode { get; set; }

        [JsonProperty("title")]
        public MediaTitle Title { get; set; }

        [JsonProperty("startDate")]
        public Date StartDate { get; set; }

        [JsonProperty("endDate")]
        public Date EndDate { get; set; }

        [JsonProperty("season")]
        public string Season { get; set; }

        [JsonProperty("seasonYear")]
        public int SeasonYear { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("characters")]
        public Characters Characters { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("episodes")]
        public int Episodes { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("volumes")]
        public int? Volumes { get; set; }

        [JsonProperty("chapters")]
        public int? Chapters { get; set; }

        [JsonProperty("isAdult")]
        public bool IsAdult { get; set; }

        [JsonProperty("genres")]
        public List<string> Genres { get; set; }

        [JsonProperty("tags")]
        public Tag[] Tags { get; set; }

        [JsonProperty("countryOfOrigin")]
        public string CountryOfOrigin { get; set; }

        [JsonProperty("hashtag")]
        public object Hashtag { get; set; }

        [JsonProperty("trailer")]
        public Trailer Trailer { get; set; }

        [JsonProperty("coverImage")]
        public CoverImage CoverImage { get; set; }

        [JsonProperty("bannerImage")]
        public Uri BannerImage { get; set; }

        [JsonProperty("synonyms")]
        public object[] Synonyms { get; set; }

        [JsonProperty("averageScore")]
        public int AverageScore { get; set; }

        [JsonProperty("meanScore")]
        public int MeanScore { get; set; }

        [JsonProperty("popularity")]
        public int Popularity { get; set; }

        [JsonProperty("favourites")]
        public int Favourites { get; set; }

        [JsonProperty("rankings")]
        public List<Rankings> Rankings { get; set; }

        [JsonProperty("siteUrl")]
        public Uri SiteUrl { get; set; }

        [JsonProperty("studios")]
        public Studios Studios { get; set; }

        [JsonProperty("relations")]
        public Relations Relations { get; set; }

        [JsonProperty("recommendations")]
        public Recommendations Recommendations { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("reviews")]
        public Reviews Reviews { get; set; }
    }

    public partial class CoverImage
    {
        [JsonProperty("large")]
        public Uri Large { get; set; }

        [JsonProperty("extraLarge")]
        public Uri ExtraLarge { get; set; }

        [JsonProperty("medium")]
        public Uri Medium { get; set; }
    }

    public partial class NextAiringEpisode
    {
        [JsonProperty("episode")]
        public int Episode { get; set; }

        [JsonProperty("airingAt")]
        public int AiringAt { get; set; }
    }

    public partial class Date
    {
        [JsonProperty("day")]
        public int Day { get; set; }

        [JsonProperty("month")]
        public int Month { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }
    }

    public partial class Rankings
    {
        [JsonProperty("rank")]
        public long Rank { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("allTime")]
        public bool AllTime { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("season")]
        public string Season { get; set; }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("year")]
        public long? Year { get; set; }
    }

    public partial class Recommendations
    {
        [JsonProperty("nodes")]
        public RecommendationsNode[] Nodes { get; set; }
    }

    public partial class RecommendationsNode
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("userRating")]
        public string UserRating { get; set; }

        [JsonProperty("mediaRecommendation")]
        public MediaRecommendation MediaRecommendation { get; set; }
    }

    public partial class MediaRecommendation
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public MediaRecommendationTitle Title { get; set; }

        [JsonProperty("coverImage")]
        public CoverImage CoverImage { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("siteUrl")]
        public string SiteUrl { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class MediaRecommendationTitle
    {
        [JsonProperty("userPreferred")]
        public string UserPreferred { get; set; }
    }

    public partial class Relations
    {
        [JsonProperty("edges")]
        public Edge[] Edges { get; set; }
    }

    public partial class Edge
    {
        [JsonProperty("relationType")]
        public string RelationType { get; set; }

        [JsonProperty("node")]
        public MediaRecommendation Node { get; set; }
    }

    public partial class Reviews
    {
        [JsonProperty("nodes")]
        public List<ReviewsNode> Nodes { get; set; }
    }

    public partial class ReviewsNode
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("ratingAmount")]
        public int RatingAmount { get; set; }

        [JsonProperty("userRating")]
        public string UserRating { get; set; }
    }

    public partial class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("avatar")]
        public Avatar Avatar { get; set; }
    }

    public partial class Avatar
    {
        [JsonProperty("large")]
        public Uri Large { get; set; }
    }

    public partial class Stats
    {
        [JsonProperty("statusDistribution")]
        public StatusDistribution[] StatusDistribution { get; set; }

        [JsonProperty("scoreDistribution")]
        public ScoreDistribution[] ScoreDistribution { get; set; }
    }

    public partial class ScoreDistribution
    {
        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }

    public partial class StatusDistribution
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class Tag
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isMediaSpoiler")]
        public bool IsMediaSpoiler { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }
    }

    public partial class MediaTitle
    {
        [JsonProperty("userPreferred")]
        public string UserPreferred { get; set; }

        [JsonProperty("english")]
        public string English { get; set; }

        [JsonProperty("romaji")]
        public string Romaji { get; set; }

        [JsonProperty("native")]
        public string Native { get; set; }
    }

    public partial class Trailer
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Studios
    {
        [JsonProperty("nodes")]
        public List<StudioNode> Nodes { get; set; }
    }

    public partial class StudioNode
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isAnimationStudio")]
        public bool IsAnimationStudio { get; set; }
    }

    public partial class Pagination
    {
        [JsonProperty("mediaList")]
        public MediaList[] MediaList { get; set; }
    }

    public partial class MediaList
    {
        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("user")]
        public FollowingUser User { get; set; }
    }

    public partial class FollowingUser
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

    public partial class Characters
    {
        [JsonProperty("edges")]
        public List<CharacterEdge> Edges { get; set; }
    }

    public partial class CharacterEdge
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("voiceActors")]
        public List<CharacterNode> VoiceActors { get; set; }

        [JsonProperty("node")]
        public CharacterNode CharacterNode { get; set; }

        [JsonIgnore]
        public CharacterNode SelectedVoiceActor { get; set; }
    }

    public partial class CharacterNode
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("languageV2")]
        public string LanguageV2 { get; set; }
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

    public static partial class Initialize
    {
        public static async Task<MediaModel> FetchData(int MediaId)
        {
            
            IRequestHandler Handler = App.Current
                .Services.GetService<IRequestHandler>();
            MediaModel Model = await Handler.RequestApi<MediaModel>(
                Queries.Query.MediaQuery,
                new
                {
                    mediaId = MediaId
                }
            );
            return Model;
        }
    }
}
