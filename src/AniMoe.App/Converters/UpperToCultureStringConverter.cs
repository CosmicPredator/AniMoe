using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Converters
{
    public class UpperToCultureStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return null;
            TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
            return textinfo.ToTitleCase(value.ToString().ToLower().Replace("_", " "));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
