using AniMoe.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Helpers
{
    public class LocalSettings : ILocalSettings
    {
        private Windows.Storage.ApplicationDataContainer localSettings = 
            Windows.Storage.ApplicationData.Current.LocalSettings;

        public T ReadSettings<T>(string key)
        {
            return (T)localSettings.Values[key];
        }

        public void WriteSettings<T>(string key, T value)
        {
            localSettings.Values[key] = value;
        }

        public bool IsSettingExists(string key)
        {
            return localSettings.Values.ContainsKey(key);
        }
    }
}
