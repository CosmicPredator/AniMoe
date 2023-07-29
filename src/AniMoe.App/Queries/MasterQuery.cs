using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string MasterQuery = @"
            query {
              Viewer {
                name
                avatar {
                  large
                  medium
                }
                id
              }
              MediaTagCollection {
                name
                id
                description
                category
              }
              GenreCollection
              SourceList: __type (name: ""MediaSource"") {
                name
                enumValues{
                  name
                }
              }
             MediaSortEnum: __type(name: ""MediaSort""){
                name
                enumValues{
                  name
                }
              }
            MediaSeasonEnum: __type (name: ""MediaSeason"") {
                name
                enumValues{
                  name
                }
              }
            MediaFormatEnum: __type (name: ""MediaFormat"") {
                name
                enumValues{
                  name
                }
              }
            }
            ";
    }
}
