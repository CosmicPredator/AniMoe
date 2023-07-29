using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string StatusListQuery = @"
            query ($userId: Int) {
              AnimeStatusList: MediaListCollection(userId: $userId, type: ANIME, sort: STATUS) {
                lists {
                  name
                  isCustomList
                }
              }
              MangaStatusList: MediaListCollection(userId: $userId, type: MANGA, sort: STATUS) {
                lists {
                  name
                  isCustomList
                }
              }
            }
            ";
    }
}
