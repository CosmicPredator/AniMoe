using AniMoe.App.ViewModels;
using AniMoe.App.Views;
using LiveChartsCore;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace AniMoe.App.Controls
{
    public sealed partial class StatsPage : Page
    {
        public NavObject NavArgs { get; set; }
        public MediaViewViewModel ViewModel { get; set; }

        public List<ISeries> StatusSeries { get; set; }

        TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavArgs = e.Parameter as NavObject;
            ViewModel = NavArgs.Vm;
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }

        public StatsPage()
        {
            this.InitializeComponent();
        }

        public void SetLegendTextPaint() {
            if (App.Current.RequestedTheme == ApplicationTheme.Light )
            {
                PieChartView.LegendTextPaint = new SolidColorPaint
                {
                    Color = new SKColor(0, 0, 0),
                };
            } else
            {
                PieChartView.LegendTextPaint = new SolidColorPaint
                {
                    Color = new SKColor(255, 255, 255),
                };
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetLegendTextPaint();
            StatusSeries = new();
            foreach(var i in ViewModel.Model.Data.Media.Stats.StatusDistribution )
            {
                StatusSeries.Add(new PieSeries<int>
                {
                    Values = new List<int>() { i.Amount },
                    InnerRadius = 50,
                    Name = textinfo.ToTitleCase(i.Status.ToLower())
                });
            }
            PieChartView.Series = StatusSeries;
        }
    }
}
