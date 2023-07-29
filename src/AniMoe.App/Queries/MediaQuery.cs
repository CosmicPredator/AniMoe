using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string MediaQuery = @"
            query ($mediaId: Int){
            Page{
                mediaList(isFollowing: true, mediaId: $mediaId){
                    score(format: POINT_10_DECIMAL)
                    status
                  user {
                    name
                    avatar {
                      medium
                    }
                  }
                }
              }
              Media(id: $mediaId){
                ...MediaInfo
                relations {
                  ...RelatedMedia
                }
                recommendations {
                  ... Recommendation
                }
                stats {
                  ...ScoreList
                }
                reviews {
                  ...ReviewList
                }
              }
            }

            fragment CharacterInfo on CharacterConnection {
              edges {
                role
                voiceActors {
                  id
                  name {
                    userPreferred
                  }
                  image {
                    large
                  }
                  languageV2
                }
                node {
                  id
                  image {
                    large
                  }
                  name {
                    userPreferred
                  }
                }
              }
            }

            fragment ReviewList on ReviewConnection {
              nodes {
                id
                user {
                  id name
                  avatar { large }
                }
                summary
                rating
                ratingAmount
                userRating
              }
            }

            fragment ScoreList on MediaStats {
              statusDistribution {
                amount status
              }
              scoreDistribution {
                score amount
              }
            }

            fragment Recommendation on RecommendationConnection {
              nodes {
                id
                rating
                userRating
                mediaRecommendation {
                  ...MediaTitle
                }
              }
            }

            fragment MediaRanking on MediaRank {
              rank
              type
              allTime
              format
              season
              context
              year
            }

            fragment RelatedMedia on MediaConnection{
              edges {
                relationType
                node {
                  ...MediaTitle
                }
              }
            }

            fragment MediaTitle on Media {
              id 
              title {
                userPreferred
              }
              coverImage {
                medium
                large
                extraLarge
              }
              format
              siteUrl
              status
            }

            fragment MediaInfo on Media{
              id
              type
              title {
                userPreferred
                english
                romaji
                native
              }
              nextAiringEpisode{
                  episode
                  airingAt
              }
              startDate{day month year}
              endDate {day month year}
              season
              seasonYear
              description
              format
              status
              studios {
                nodes{
                  name
                  isAnimationStudio
                }
              }
              episodes
              duration
              chapters
              characters(sort: [ROLE, RELEVANCE]) {
                ...CharacterInfo
              }
              volumes
              source
              isAdult
              rankings {
                ...MediaRanking
              }
              genres
              tags {
                name
                id
                description
                isMediaSpoiler
                rank
              }
              countryOfOrigin
              hashtag
              trailer {
                id
              }
              coverImage {large extraLarge medium}
              bannerImage
              synonyms
              averageScore
              meanScore
              popularity
              favourites
              siteUrl
            }
            ";
    }
}
