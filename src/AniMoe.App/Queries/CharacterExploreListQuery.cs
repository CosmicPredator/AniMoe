using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string CharacterListExploreQuery = @"
            query (
              $page: Int,
              $search: String,
              $isBirthday: Boolean
            ){
              Page (page: $page) {
                pageInfo{
                  currentPage
                  hasNextPage
                }
                characters (
                  sort: FAVOURITES_DESC,
                  search: $search,
                  isBirthday: $isBirthday
                ){
                  id
                  name {
                    userPreferred
                  }
                  image {
                    large
                  }
                }
              }
            }";
    }
}
