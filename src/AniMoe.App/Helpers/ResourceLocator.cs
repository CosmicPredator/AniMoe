using AniMoe.App.Services;
using Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Helpers
{
    public class ResourceLocator : IResourceLocator
    {
        readonly ResourceLoader resource = new ResourceLoader("Constants");
        public string GetAuthUrl()
        {
            return resource.GetString("authUrl");
        }

        public string GetBaseUrl()
        {
            return resource.GetString("baseUrl");
        }
    }
}
