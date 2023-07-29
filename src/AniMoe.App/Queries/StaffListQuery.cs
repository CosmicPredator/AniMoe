using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string StaffListQuery = @"
            query ($mediaId: Int, $pageNum: Int) {
              Media(id: $mediaId) {
                type
                staff (page: $pageNum) {
                  pageInfo{
                    hasNextPage
                    currentPage
                    perPage
                  }
                  edges {
                    role
                    node {
                      id
                      image{
                        large
                      }
                      name {
                        userPreferred
                      }
                    }
                  }
                }
              }
            }
            ";
    }
}
