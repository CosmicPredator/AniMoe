using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.UI.Xaml;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Controls.UserViewControls.ViewModels
{
    public partial class UserStatViewModel : ObservableObject
    {
        public ICartesianAxis[] ScoreXAxes = new ICartesianAxis[]
        {
            new Axis
            {
                Labels = new List<string>
                {
                    "15", "20", "30", "40", "45", "50", "55",
                        "60", "65", "70", "75", "80", "85", "90", "95", "100"
                },
                ForceStepToMin = true,
                LabelsPaint = new SolidColorPaint(SKColors.White)
            },
        };

        public ICartesianAxis[] ScoreYAxes = new ICartesianAxis[]
        {
            new Axis
            {
                ShowSeparatorLines = false,
                LabelsPaint = new SolidColorPaint(SKColors.Transparent),
                SeparatorsPaint = new SolidColorPaint(SKColors.Transparent),
                IsVisible = false
            }
        };

        [ObservableProperty]
        private ISeries[] _series;

        [ObservableProperty]
        private ISeries[] _lineSeries;

        [ObservableProperty]
        private ISeries[] _pieChatSeries;

        [ObservableProperty]
        private SolidColorPaint _legendPaint;

        private SKColor ConvertAccent()
        {
            var accentColorResource = Application.Current.Resources["SystemAccentColorLight2"];
            var accentColor = (Windows.UI.Color)accentColorResource;
            var skColor = new SKColor(accentColor.R, accentColor.G, accentColor.B, accentColor.A);
            return skColor;
        }

        public UserStatViewModel()
        {
            LegendPaint = new SolidColorPaint(SKColors.White);
            Series = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Values = new int[] {1, 1, 1, 4, 2, 4, 4, 17, 13, 15, 5, 28, 17, 3, 7, 9},
                    MaxBarWidth = 20,
                    Fill = new SolidColorPaint(ConvertAccent()),
                    Rx = 50,
                    Ry = 50,
                    DataPadding = new LvcPoint(0, 0.3f),
                    XToolTipLabelFormatter =
                                   (chartPoint) => String.Empty
                }
            };

            LineSeries = new ISeries[]
            {
                new LineSeries<int>
                {
                    Values = new int[] {1, 1, 1, 4, 2, 4, 4, 17, 13, 15, 5, 28, 17, 3, 7, 9},
                    DataPadding = new LvcPoint(0, 0.3f),
                    Fill = new SolidColorPaint(SKColors.Transparent),
                    GeometryStroke = new SolidColorPaint(ConvertAccent()),
                    GeometryFill = new SolidColorPaint(ConvertAccent()),
                    Stroke = new SolidColorPaint(ConvertAccent())
                    {
                        StrokeThickness = 3,
                    },
                    XToolTipLabelFormatter =
                           (chartPoint) => String.Empty
                }
            };

            PieChatSeries = new ISeries[]
            {
                new PieSeries<int> {Values = new int[] {2}, Name = "TV"},
                new PieSeries<int> {Values = new int[] {4}, Name = "OVA"},
                new PieSeries<int> {Values = new int[] {1}, Name = "Movie"},
                new PieSeries<int> {Values = new int[] {4}, Name = "Japan"},
                new PieSeries<int> {Values = new int[] {3}, Name = "Planning"}
            };
        }
    }
}
