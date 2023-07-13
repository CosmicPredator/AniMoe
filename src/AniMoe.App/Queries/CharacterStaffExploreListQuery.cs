using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string CharacterStaffListExploreQuery = @"
            query (
              $page: Int,
              $search: String,
              $isBirthday: Boolean,
              $isCharacter: Boolean!
            ){
              Character: Page (page: $page) @include(if: $isCharacter) {
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
              Staff: Page (page: $page) @skip(if: $isCharacter) {
                pageInfo{
                  currentPage
                  hasNextPage
                }
                staff (
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
