using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace AniMoe.App.Converters
{
    public class LikeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ( value.ToString() == "UP_VOTE" )
            {
                return (SolidColorBrush)Application.Current.Resources["SystemAccentColorLight2"];
            } else
            {
                return (SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class DisLikeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if( value.ToString() == "DOWN_VOTE" )
            {
                return new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColorLight2"]);
            }
            else
            {
                return (SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
