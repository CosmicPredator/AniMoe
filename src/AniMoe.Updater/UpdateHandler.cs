using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace AniMoe.Updater
{
    public class UpdateHandler
    {
        private HttpClient _httpClient;
        private string _appVersionString;
        private string _accessToken;
        private string _baseUrl;

        public UpdateHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _appVersionString = string.Format("Version: {0}.{1}.{2}",
                    Package.Current.Id.Version.Major,
                    Package.Current.Id.Version.Minor,
                    Package.Current.Id.Version.Build);

            _baseUrl = @"https://api.github.com/repos/CosmicPredator/AniMoe/releases/latest";

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
            _httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
            _httpClient.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0");
        }

        public async Task<GithubModel> CheckLatestRelease()
        {
            var request = await _httpClient.GetAsync(_baseUrl);
            return JsonConvert.DeserializeObject<GithubModel>(await request.Content.ReadAsStringAsync(), 
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }
}
