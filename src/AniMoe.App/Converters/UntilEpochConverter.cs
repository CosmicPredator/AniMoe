using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace AniMoe.App.Converters
{
    public class UntilEpochConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if( (int?)value == null )
            {
                return "None";
            } else
            {
                DateTime ModifiedDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    .ToLocalTime()
                    .AddSeconds(System.Convert.ToDouble(value));
                TimeSpan diff = ModifiedDate.Subtract(DateTime.Now);
                return $"{(diff.Days < 1 ? "" : $"{diff.Days}d ")}{(diff.Hours < 1 ? "" : $"{diff.Hours}h ")}{(diff.Minutes < 1 ? "" : $"{diff.Minutes}m")}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
