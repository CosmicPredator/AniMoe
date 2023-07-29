using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string ReviewExploreQuery = @"
            query (
              $page: Int
            ){
              Page (page: $page){
                pageInfo{
                  hasNextPage
                  currentPage
                }
                reviews (sort: CREATED_AT_DESC){
                  id
                  media{
                    id
                    bannerImage
                    title{
                      userPreferred
                    }
                  }
                  user{
                    id
                    name
                  }
                  summary
                  rating
                  ratingAmount
                }
              }
            }
            @";
    }
}
