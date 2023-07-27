using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string StaffQuery = @"
            query($id: Int){
              Staff(id: $id){
                age
                dateOfBirth{
                  day
                  month
                  year
                }
                description
                favourites
                gender
                homeTown
                id
                image{
                  large
                }
                name{
                  userPreferred
                  native
                  alternative
                }
                yearsActive
              }
            }";
    }
}
