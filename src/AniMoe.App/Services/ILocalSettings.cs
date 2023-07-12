using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Services
{
    public interface ILocalSettings
    {
        public T ReadSettings<T>(string key);
        public void WriteSettings<T>(string key, T value);
        public bool IsSettingExists(string key);
    }
}
