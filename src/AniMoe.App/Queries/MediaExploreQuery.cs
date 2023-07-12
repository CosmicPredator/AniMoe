using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string MediaExploreQuery = @"
            query (
              $page:Int,
              $type: MediaType,
              $sortOptions: [MediaSort] = [TRENDING_DESC],
              $airingStatus: MediaStatus,
              $season: MediaSeason,
              $format: MediaFormat,
              $country: CountryCode,
              $sourceMaterial: MediaSource,
              $year: Int,
              $episodesStartRange: Int,
              $episodesEndRange: Int,
              $durationStartRange: Int,
              $durationEndRange: Int,
              $chapterStartRange: Int,
              $chapterEndRange: Int,
              $volumeStartRange: Int,
              $volumeEndRange: Int,
              $yearStartRange: FuzzyDateInt,
              $yearEndRange: FuzzyDateInt,
              $genresIn: [String],
              $tagsIn: [String],
              $genresNotIn: [String],
              $tagsNotIn: [String],
              $isAdult: Boolean, 
              $isDoujin: Boolean,
              $onList: Boolean,
              $search: String
            ) {
              Page(page: $page) {
                pageInfo {
                  perPage
                  hasNextPage
                  currentPage
                }
                AnimeManga: media(
                  type: $type
                  sort: $sortOptions, 
                  status: $airingStatus,
                  season: $season,
                  format: $format,
                  countryOfOrigin: $country,
                  source: $sourceMaterial,
                  seasonYear: $year, 
                  episodes_greater: $episodesStartRange,
                  episodes_lesser: $episodesEndRange,
                  duration_greater: $durationStartRange,
                  duration_lesser: $durationEndRange,
                  chapters_greater: $chapterStartRange,
                  chapters_lesser: $chapterEndRange,
                  volumes_greater: $volumeStartRange
                  volumes_lesser: $volumeEndRange,
                  startDate_greater: $yearStartRange,
                  startDate_lesser: $yearEndRange,
                  genre_in: $genresIn,
                  genre_not_in: $genresNotIn,
                  tag_in: $tagsIn,
                  tag_not_in: $tagsNotIn,
                  isAdult: $isAdult,
                  isLicensed: $isDoujin,
                  onList: $onList,
                  search: $search
                  ){
                  ...MediaInfo
                }
              }
            }

            fragment MediaInfo on Media {
              id
              type
              coverImage {
                large
              }
              title {
                userPreferred
              }
            }";
    }
}
