using AniMoe.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Models.UserModel
{

    public partial class UserModel
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("User")]
        public User User { get; set; }
    }

    public partial class User
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("about")]
        public string About { get; set; }

        [JsonProperty("avatar")]
        public Avatar Avatar { get; set; }

        [JsonProperty("bannerImage")]
        public Uri BannerImage { get; set; }

        [JsonProperty("isFollowing")]
        public bool IsFollowing { get; set; }

        [JsonProperty("isFollower")]
        public bool IsFollower { get; set; }

        [JsonProperty("isBlocked")]
        public bool IsBlocked { get; set; }

        [JsonProperty("siteUrl")]
        public Uri SiteUrl { get; set; }

        [JsonProperty("donatorTier")]
        public long DonatorTier { get; set; }

        [JsonProperty("donatorBadge")]
        public string DonatorBadge { get; set; }

        [JsonProperty("moderatorRoles")]
        public object ModeratorRoles { get; set; }

        [JsonProperty("statistics")]
        public Statistics Statistics { get; set; }

        [JsonProperty("favourites")]
        public Favourites Favourites { get; set; }
    }

    public partial class Avatar
    {
        [JsonProperty("large")]
        public Uri Large { get; set; }
    }

    public partial class Favourites
    {
        [JsonProperty("anime")]
        public FavouritesAnime Anime { get; set; }

        [JsonProperty("manga")]
        public FavouritesAnime Manga { get; set; }

        [JsonProperty("characters")]
        public Characters Characters { get; set; }

        [JsonProperty("staff")]
        public Characters Staff { get; set; }
    }

    public partial class FavouritesAnime
    {
        [JsonProperty("nodes")]
        public AnimeNode[] Nodes { get; set; }
    }

    public partial class AnimeNode
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public Title Title { get; set; }

        [JsonProperty("coverImage")]
        public Avatar CoverImage { get; set; }
    }

    public partial class Title
    {
        [JsonProperty("userPreferred")]
        public string UserPreferred { get; set; }
    }

    public partial class Characters
    {
        [JsonProperty("nodes")]
        public CharactersNode[] Nodes { get; set; }
    }

    public partial class CharactersNode
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public Title Name { get; set; }

        [JsonProperty("image")]
        public Avatar Image { get; set; }
    }

    public partial class Statistics
    {
        [JsonProperty("anime")]
        public StatisticsAnime Anime { get; set; }

        [JsonProperty("manga")]
        public StatisticsAnime Manga { get; set; }
    }

    public partial class StatisticsAnime
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("meanScore")]
        public double MeanScore { get; set; }

        [JsonProperty("standardDeviation")]
        public double StandardDeviation { get; set; }

        [JsonProperty("minutesWatched")]
        public double MinutesWatched { get; set; }

        [JsonProperty("episodesWatched")]
        public long EpisodesWatched { get; set; }

        [JsonProperty("chaptersRead")]
        public long ChaptersRead { get; set; }

        [JsonProperty("volumesRead")]
        public long VolumesRead { get; set; }

        [JsonProperty("scores")]
        public Country[] Scores { get; set; }

        [JsonProperty("lengths")]
        public Country[] Lengths { get; set; }

        [JsonProperty("formats")]
        public Country[] Formats { get; set; }

        [JsonProperty("countries")]
        public Country[] Countries { get; set; }

        [JsonProperty("releaseYears")]
        public Country[] ReleaseYears { get; set; }

        [JsonProperty("startYears")]
        public Country[] StartYears { get; set; }

        [JsonProperty("genres")]
        public Genre[] Genres { get; set; }
    }

    public partial class Genre
    {
        [JsonProperty("genre")]
        public string GenreName { get; set; }

        [JsonProperty("count")]
        public string GenreCount { get; set; }
    }

    public partial class Country
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("meanScore")]
        public double MeanScore { get; set; }

        [JsonProperty("chaptersRead")]
        public long ChaptersRead { get; set; }

        [JsonProperty("minutesWatched")]
        public long MinutesWatched { get; set; }

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryCountry { get; set; }

        [JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
        public string Format { get; set; }

        [JsonProperty("releaseYear", NullValueHandling = NullValueHandling.Ignore)]
        public long? ReleaseYear { get; set; }

        [JsonProperty("startYear", NullValueHandling = NullValueHandling.Ignore)]
        public long? StartYear { get; set; }
    }

    public static partial class Initialize
    {
        public static async Task<UserModel> FetchData(int userId, bool isSelf)
        {
            IRequestHandler Handler = App.Current.Services.GetService<IRequestHandler>();
            UserModel Model = await Handler.RequestApi<UserModel>(
                Queries.Query.UserInfoQuery,
                new
                {
                    userId,
                    isSelf
                }
            );
            return Model;
        }
    }
}
