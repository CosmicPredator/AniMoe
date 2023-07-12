using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Services
{
    public interface IRequestHandler
    {
        Task<T> RequestApi<T>(string query, object variables = null);
        Task DownloadCoverImage(string url, long id, string title);
    }
}
