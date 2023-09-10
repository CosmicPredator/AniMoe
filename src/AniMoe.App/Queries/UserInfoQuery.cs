using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string UserInfoQuery = @"
            query ($userId: Int, $isSelf: Boolean = false) {
              User(id: $userId) {
                id
                name
                about
                avatar {
                  large
                }
                bannerImage
                isFollowing @skip(if: $isSelf)
                isFollower @skip(if: $isSelf)
                isBlocked
                siteUrl
                donatorTier
                donatorBadge
                moderatorRoles
                statistics {
                  anime {
                    ...UserStats
                  }
                  manga {
                    ...UserStats
                  }
                }
              }
            }

            fragment UserStats on UserStatistics {
              count
              meanScore
              standardDeviation
              minutesWatched
              episodesWatched
              chaptersRead
              volumesRead
              scores {
                ...ScoreStats
              }
              lengths {
                ...LengthStats
              }
              genres {
                ...GenreStats
              }
              tags {
                ...TagStats
              }
              formats {
                ...FormatStats
              }
              countries {
                ...CountryStats
              }
              releaseYears {
                ...ReleaseYearStats
              }
              startYears {
                ...WatchYearStats
              }
            }

            fragment ScoreStats on UserScoreStatistic {
              count
              minutesWatched
              meanScore
            }

            fragment LengthStats on UserLengthStatistic {
              count
              minutesWatched
              meanScore
            }

            fragment GenreStats on UserGenreStatistic {
              genre
              count
              meanScore
              minutesWatched
              mediaIds
            }

            fragment TagStats on UserTagStatistic {
              count
              meanScore
              minutesWatched
              mediaIds
              tag {
                id
                name
              }
            }

            fragment FormatStats on UserFormatStatistic {
              count
              meanScore
              minutesWatched
              format
            }

            fragment CountryStats on UserCountryStatistic {
              count
              meanScore
              minutesWatched
              country
            }

            fragment ReleaseYearStats on UserReleaseYearStatistic {
              count
              meanScore
              minutesWatched
              releaseYear
            }

            fragment WatchYearStats on UserStartYearStatistic {
              count
              meanScore
              minutesWatched
              startYear
            }";
    }
}
