using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AniMoe.App.Services;
using System.Net.Http;
using Windows.Storage;
using System.Threading;
using Serilog;

namespace AniMoe.App.Helpers
{
    public class RequestHandler : IRequestHandler
    {
        private ILocalSettings _settings;
        private IResourceLocator _locator;
        private HttpClient _client;
        private INotificationService _ntService;

        public RequestHandler(ILocalSettings settings, 
            IResourceLocator locator, HttpClient client, INotificationService ntService)
        {
            _settings = settings;
            _locator = locator;
            _client = client;
            _ntService = ntService;
        }

        public async Task DownloadCoverImage(string url, long id, string title)
        {
            byte[] imageBytes = await _client.GetByteArrayAsync(
                new Uri(url)
            );
            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            StorageFile imageFile = await picturesFolder.CreateFileAsync(
                $"{id}.png",
                CreationCollisionOption.ReplaceExisting
            );
            await FileIO.WriteBytesAsync(imageFile, imageBytes);
            Log.Information("Image of MediaId {mediaId} saved successfully", id);
            _ntService.SendImageNotification(imageFile.Path.ToString(), url);
        }

        public async Task<T> RequestApi <T>(string query, object variables = null)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            _client.DefaultRequestHeaders.Add(
                "Authorization",
                $"Bearer {_settings.ReadSettings<string>("accessToken")}"
            );
            var jsonString = new StringContent(
                JsonConvert.SerializeObject(
                    new
                    {
                        query = query,
                        variables = variables
                    }
                ),
                Encoding.UTF8,
                "application/json"
            );
            var request = await _client.PostAsync(
                _locator.GetBaseUrl(),
                jsonString,
                cancellationTokenSource.Token
            );
            Log.Information($"Request <{typeof(T)}> to API was successful with code \"{request.StatusCode}\"");
            Log.Debug(await request.Content.ReadAsStringAsync());
            cancellationTokenSource.Cancel();
            if( request.IsSuccessStatusCode )
            {
                _client.Dispose();
                cancellationTokenSource.Dispose();
                return JsonConvert.DeserializeObject<T>(
                    await request.Content.ReadAsStringAsync(),
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    }
                );
            } else
            {
                return default;
            }
        }
    }
}
