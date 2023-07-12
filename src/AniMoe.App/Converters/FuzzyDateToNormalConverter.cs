using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AniMoe.App.Models.MediaModel;

namespace AniMoe.App.Converters
{
    public class FuzzyDateToNormalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Date date = (Date)value;
            if( date == null ||
                date.Day == 0 ||
                date.Month == 0 ||
                date.Year == 0) return null;
            DateTime formattedDate = new DateTime(date.Year, date.Month, date.Day);
            return formattedDate.ToString("ddd, dd MMM yyy ");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
