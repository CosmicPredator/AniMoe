using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Queries
{
    public static partial class Query
    {
        public static string UserActivityQuery = @"
            query ($userId: Int) {
              Page(page: 1){
                activities(userId: $userId, sort: [ID_DESC]) {
                  ...on TextActivity{
                    ...textActivity
                  }
                  ...on ListActivity {
                    ...listActivity
                  }
                  ...on MessageActivity {
                    ...messageActivity
                  }
                }
              }
            }

            fragment textActivity on TextActivity {
              id
              userId
              type
              replyCount
              text
              siteUrl
              isSubscribed
              likeCount
              isLiked
              isPinned
              createdAt
              user {
                id name avatar { medium }
              }
            }

            fragment listActivity on ListActivity {
              id
              user {
                name
                id
                avatar { medium }
              }
              type
              replyCount
              status
              progress
              isSubscribed
              likeCount
              isLiked
              isPinned
              siteUrl
              createdAt
              media {
                id
                title {userPreferred}
                coverImage{large}
              }
            }

            fragment messageActivity on MessageActivity {
              id
              recipientId
              recipient{
                name
                avatar{medium}
              }
              messengerId
              messenger {
                name
                avatar {medium}
              }
              type
              replyCount
              message
              isSubscribed
              likeCount
              isLiked
              isPrivate
              siteUrl
              createdAt
            }";
    }
}
