using System;
using System.Collections.Generic;
using System.Linq;

namespace AniMoe.App.Helpers
{
    public enum MediaType
    {
        ANIME,
        MANGA
    }
    public class Country
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }

    public class Status
    {
        public string StatusName { get; set; }
        public string StatusEnumCode { get; set; }
    }

    public class Format
    {
        public string FormatName { get; set; }
        public string FormatEnumCode { get; set; }
        public MediaType Type { get; set; }
    }

    public static partial class TagsGenres
    {
        public static List<Country> CountryList = new()
        {
            new Country
            {
                CountryName = "Select One",
                CountryCode = null
            },
            new Country
            {
                CountryName = "Japan",
                CountryCode = "JP"
            },
            new Country
            {
                CountryName = "South Korea",
                CountryCode = "KR"
            },
            new Country
            {
                CountryName = "China",
                CountryCode = "CN"
            },
            new Country
            {
                CountryName = "Taiwan",
                CountryCode = "TW"
            }
        };

        public static List<Status> StatusList = new()
        {
            new Status
            {
                StatusName = "Select One",
                StatusEnumCode = null
            },
            new Status
            {
                StatusName = "Finished",
                StatusEnumCode = "FINISHED"
            },
            new Status
            {
                StatusName = "Releasing",
                StatusEnumCode = "RELEASING"
            },
            new Status
            {
                StatusName = "Not Yet Released",
                StatusEnumCode = "NOT_YET_RELEASED"
            },
            new Status
            {
                StatusName = "Cancelled",
                StatusEnumCode = "CANCELLED"
            },
            new Status
            {
                StatusName = "Hiatus",
                StatusEnumCode = "HIATUS"
            }
        };

        public static List<Format> FormatList = new()
        {
            new Format
            {
                FormatName = "Select One",
                FormatEnumCode = null,
                Type = MediaType.ANIME | MediaType.MANGA
            },
            new Format
            {
                FormatName = "TV Show",
                FormatEnumCode = "TV",
                Type = MediaType.ANIME
            },
            new Format
            {
                FormatName = "TV Short",
                FormatEnumCode = "TV_SHORT",
                Type = MediaType.ANIME
            },
            new Format
            {
                FormatName = "Movie",
                FormatEnumCode = "MOVIE",
                Type = MediaType.ANIME
            },
            new Format
            {
                FormatName = "Special",
                FormatEnumCode = "SPECIAL",
                Type = MediaType.ANIME
            },
            new Format
            {
                FormatName = "OVA",
                FormatEnumCode = "OVA",
                Type = MediaType.ANIME
            },
            new Format
            {
                FormatName = "ONA",
                FormatEnumCode = "ONA",
                Type = MediaType.ANIME
            },
            new Format
            {
                FormatName = "Music",
                FormatEnumCode = "MUSIC",
                Type = MediaType.ANIME
            },
            new Format
            {
                FormatName = "Manga",
                FormatEnumCode = "MANGA",
                Type = MediaType.MANGA
            },
            new Format
            {
                FormatName = "Novel",
                FormatEnumCode = "NOVEL",
                Type = MediaType.MANGA
            },
            new Format
            {
                FormatName = "One Shot",
                FormatEnumCode = "ONE_SHOT",
                Type = MediaType.MANGA
            }
        };

        public static List<string> MediaSourceList = new List<string>()
        {
            "SELECT_ONE",
            "ANIME",
            "COMIC",
            "DOUJINSHI",
            "GAME",
            "LIGHT_NOVEL",
            "LIVE_ACTION",
            "MANGA",
            "MULTIMEDIA_PROJECT",
            "NOVEL",
            "ORIGINAL",
            "OTHER",
            "PICTURE_BOOK",
            "VIDEO_GAME",
            "VISUAL_NOVEL",
            "WEB_NOVEL"
        };
    }
}
