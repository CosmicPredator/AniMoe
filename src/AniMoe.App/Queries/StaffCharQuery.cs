using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public partial class Query
    {
        public static string StaffCharQuery = @"
            query($id: Int, $onList: Boolean = false){
              Staff(id: $id){
                characterMedia(page: 1, sort: START_DATE_DESC, onList: $onList){
                  pageInfo{
                    hasNextPage
                  }
                  edges{
                    characterRole
                    characters{
                      id
                      name {
                        userPreferred
                      }
                      image {
                        large
                      }
                    }
                    node{
                      id
                      title{
                        userPreferred
                      }
                      coverImage{
                        large
                      }
                      type
                    }
                  }
                }
              }
            }";
    }
}
