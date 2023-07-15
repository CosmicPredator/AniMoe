using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string ReviewQuery = @"
            query($id: Int){
              Review(id: $id){
                user{
                  name
                  id
                }
                media{
                  bannerImage
                  title{
                    userPreferred
                  }
                }
                body
                rating
                ratingAmount
                createdAt
                score
                userRating
              }
            }";
    }
}
