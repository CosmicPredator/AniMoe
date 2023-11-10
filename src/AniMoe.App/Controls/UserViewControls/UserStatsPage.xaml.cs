using CommunityToolkit.Labs.WinUI;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AniMoe.App.Controls.UserViewControls
{
    public sealed partial class UserStatsPage : Page
    {
        public ObservableCollection<int> Values { get; set; } = new();

        public ISeries[] Series { get; set; }

        public ICartesianAxis[] XAxes = new ICartesianAxis[]
        {
            new Axis
            {
                Labels = new List<string>
                {
                    "15", "20", "30", "40", "45", "50", "55", "60", "65", "70", "75", "80", "85", "90", "95", "100"
                },
                ForceStepToMin = true
            },
        };

        public ICartesianAxis[] YAxes = new ICartesianAxis[]
        {
            new Axis
            {
                ShowSeparatorLines = false,
                LabelsPaint = new SolidColorPaint(SKColors.Transparent),
                SeparatorsPaint = new SolidColorPaint(SKColors.Transparent),
                IsVisible = false
            }
        };

        public UserStatsPage()
        {
            this.InitializeComponent();
            DataContext = this;
            TypeSegment.SelectionChanged += Segmented_SelectionChanged;
        }

        private void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as Segmented).SelectedIndex)
            {
                case 0:
                    Chart.Series = new ISeries[]
                    {
                        new ColumnSeries<int>
                        {
                            Values = new int[] {1, 1, 1, 4, 2, 4, 4, 17, 13, 15, 5, 28, 17, 3, 7, 9},
                            MaxBarWidth = 25,
                            Rx = 50,
                            Ry = 50,
                            DataPadding = new LvcPoint(0, 0.3f),
                            XToolTipLabelFormatter =
                                        (chartPoint) => String.Empty
                        }
                    };
                    break;
                case 1:
                    Chart.Series = new ISeries[]
                    {
                        new ColumnSeries<int>
                        {
                            Values = new int[] {5, 0, 5, 19, 15, 11, 18, 50, 51, 52, 22, 124, 70, 17, 49, 37},
                            MaxBarWidth = 25,
                            Rx = 50,
                            Ry = 50,
                            DataPadding = new LvcPoint(0, 0.3f),
                            XToolTipLabelFormatter =
                                        (chartPoint) => String.Empty
                        }
                    };
                    break;
            }
        }
    }
}
