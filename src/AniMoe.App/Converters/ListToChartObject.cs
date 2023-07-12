using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using Microsoft.UI.Xaml;

namespace AniMoe.App.Converters
{
    public class ListToChartObject : IValueConverter
    {

        private SKColor ConvertAccent()
        {
            var accentColorResource = Application.Current.Resources["SystemAccentColorLight2"];
            var accentColor = (Windows.UI.Color)accentColorResource;
            var skColor = new SKColor(accentColor.R, accentColor.G, accentColor.B, accentColor.A);
            return skColor;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new List<ISeries>
            {
                new ColumnSeries<int>
                {
                    Name = "",
                    Values = (List<int>)value,
                    Fill = new SolidColorPaint(ConvertAccent()),
                    MaxBarWidth = 20,
                    Rx = 50,
                    Ry = 50,
                    DataPadding = new LvcPoint(0, 0.3f),
                    XToolTipLabelFormatter =
                            (chartPoint) => String.Empty
                }
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
