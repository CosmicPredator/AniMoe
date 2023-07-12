using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Converters
{

    /* This Converter helps to convert the intermediate state of checkbox to
     * "intermediate" string value. So, this checkbox values are covnerted as
     * strings and handles as strings.
     * Used in ExploreView.xaml.cs */
    public class BoolToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if( (string)value == "intermediate" )
            {
                return null;
            }
            else return System.Convert.ToBoolean(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if( value == null )
            {
                return "intermediate";
            }
            else return value.ToString();
        }
    }
}
