using AniMoe.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Models.StaffModel
{
    public partial class StaffModel
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("Staff")]
        public Staff Staff { get; set; }
    }

    public partial class Staff
    {
        [JsonProperty("age")]
        public long Age { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateOfBirth DateOfBirth { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("favourites")]
        public long Favourites { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("homeTown")]
        public string HomeTown { get; set; }

        [JsonProperty("isFavourite")]
        public bool IsFavourite { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("yearsActive")]
        public List<int> YearsActive { get; set; }
    }

    public partial class DateOfBirth
    {
        [JsonProperty("day")]
        public long Day { get; set; }

        [JsonProperty("month")]
        public long Month { get; set; }

        [JsonProperty("year")]
        public long Year { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("large")]
        public Uri Large { get; set; }
    }

    public partial class Name
    {
        [JsonProperty("userPreferred")]
        public string UserPreferred { get; set; }

        [JsonProperty("native")]
        public string Native { get; set; }

        [JsonProperty("alternative")]
        public List<string> Alternative { get; set; }
    }

    public static class Initialize
    {
        public static async Task<StaffModel> FetchData(int staffId)
        {
            IRequestHandler Handler = App.Current.Services.GetRequiredService<IRequestHandler>();
            StaffModel model = await Handler.RequestApi<StaffModel>(
                Queries.Query.StaffQuery,
                new
                {
                    id = staffId
                }
            );
            return model;
        }
    }
}
