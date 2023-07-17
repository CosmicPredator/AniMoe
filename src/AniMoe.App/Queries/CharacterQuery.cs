using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string CharacterQuery = @"
            query($id: Int, $onList: Boolean){
              Character(id: $id){
                isFavourite
                id
                name{
                  userPreferred
                  native
                }
                age
                gender
                favourites
                image{
                    large
                }
                description
                media (onList: $onList){
                  edges{
                    voiceActors{
                      id
                      languageV2
                      name{
                        userPreferred
                      }
                      image{
                        large
                      }
                    }
                    node{
                      mediaListEntry{
                        status
                      }
                      type
                      title{
                        userPreferred
                      }
                      coverImage{
                        large
                      }
                      id
                    }
                  }
                }
              }
            }";
    }
}
