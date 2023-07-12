using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Converters
{
    public class BoolToRedColor : IValueConverter
    {
        private SolidColorBrush ConvertAccent()
        {
            var accentColorResource = (SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"];
            return accentColorResource;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isMediSpoiler = (bool)value;
            if( !isMediSpoiler ) return ConvertAccent();
            else return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
