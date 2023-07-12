using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AniMoe.App.Helpers;

namespace AniMoe.App.Converters
{
    public class StatusEnumToNormalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string status = (string)value;
            return TagsGenres.StatusList.Where(
                x => x.StatusEnumCode == status
            ).First().StatusName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
