using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string MediaListQuery = @"
            query MediaList($userId: Int, $mediaType: MediaType, $isAnime: Boolean!) {
              MediaListCollection(userId: $userId, type: $mediaType, sort: SCORE_DESC) {
                lists {
                  name
                  isCustomList
                  entries {
                    progress
                    media{
                      ...animeInfo
                    }
                  }
                }
              }
            }

            fragment animeInfo on Media {
              id
              title {
                userPreferred
              }
              coverImage {
                large
                extraLarge
              }
              nextAiringEpisode @include(if: $isAnime) {
                airingAt
                timeUntilAiring
                episode
              }
              episodes @include(if: $isAnime)
              chapters @skip(if: $isAnime)
              format
              genres
              status
              countryOfOrigin
              tags {
                name
              }
              seasonYear
              siteUrl
            }
            ";
    }
}
