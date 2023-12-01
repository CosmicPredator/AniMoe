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
                favourites {
                  ...UserFavourites
                }
              }
            }

            fragment UserFavourites on Favourites {
              anime {
                ...MediaFragment
              }
              manga {
                ...MediaFragment
              }
              characters {
                nodes {
                  id
                  name{
                    userPreferred
                  }
                  image {
                    large
                  }
                }
              }
              staff {
                nodes {
                  id
                  name {
                    userPreferred
                  }
                  image {
                    large
                  }
                }
              }
            }

            fragment MediaFragment on MediaConnection {
              nodes {
                id
                title {
                  userPreferred
                }
                coverImage {
                  large
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
              genres {
                genre
                count
              }
            }

            fragment ScoreStats on UserScoreStatistic {
              count
              minutesWatched
              chaptersRead
              meanScore
            }

            fragment LengthStats on UserLengthStatistic {
              count
              minutesWatched
              chaptersRead
              meanScore
            }

            fragment FormatStats on UserFormatStatistic {
              count
              meanScore
              minutesWatched
              chaptersRead
              format
            }

            fragment CountryStats on UserCountryStatistic {
              count
              meanScore
              chaptersRead
              minutesWatched
              country
            }

            fragment ReleaseYearStats on UserReleaseYearStatistic {
              count
              meanScore
              minutesWatched
              chaptersRead
              releaseYear
            }

            fragment WatchYearStats on UserStartYearStatistic {
              count
              meanScore
              minutesWatched
              chaptersRead
              startYear
            }";
    }
}
