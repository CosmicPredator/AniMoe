using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Converters
{
    public class MinutesToDays : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TimeSpan t = TimeSpan.FromMinutes((double)value);
            return t.Days.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
