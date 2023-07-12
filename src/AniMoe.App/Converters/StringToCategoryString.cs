using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PInvoke.User32;

namespace AniMoe.App.Converters
{
    public class StringToCategoryString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string input = (string)value;
            if( input.Contains('-') )
            {
                return input.Remove(input.IndexOf('-')) + " / " + input.Substring(input.IndexOf('-') + 1);
            }
            return input;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
