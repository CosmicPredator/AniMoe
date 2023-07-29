using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string CharacterListQuery = @"
            query ($mediaId: Int, $pageNum: Int){
              Media (id: $mediaId) {
                type
                characters (
                  sort: [ROLE, RELEVANCE],
                  page: $pageNum
                ) {
                  pageInfo {
                    total
                    perPage
                    hasNextPage
                    currentPage
                  }
                  edges {
                    role
                    voiceActorRoles {
                      roleNotes
                      voiceActor{
                        id
                        name {
                          userPreferred
                        }
                        image {
                          large
                        }
                        languageV2
                      }
                    }
                    node {
                      name{
                        userPreferred
                      }
                      image {
                        large
                      }
                      id
                    }
                  }
                }
              }
            }
            ";
    }
}
