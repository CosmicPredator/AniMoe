using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AniMoe.App.Helpers;

namespace AniMoe.App.Converters
{
    public class FormatEnumToNormalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string format = (string)value;
            return TagsGenres.FormatList.Where(
                x => x.FormatEnumCode == format
            ).First().FormatName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
