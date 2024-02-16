using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AniMoe.App.Controls;
using CommunityToolkit.Labs.WinUI;
using Microsoft.UI.Xaml.Media.Animation;
using System.Diagnostics;
using AniMoe.App.Models.MediaModel;
using CommunityToolkit.Mvvm.Input;
using AniMoe.Parsers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using CommunityToolkit.WinUI;
using Microsoft.Web.WebView2.Core;
using Windows.UI.WebUI;
using Serilog;

namespace AniMoe.App.ViewModels
{
    public class MediaViewViewModel: ObservableObject
    {
        private List<int> _series;
        private int segmentedSelectedIndex;
        private MediaModel model;
        private bool loaded = false;
        private bool isLoading = true;
        private int MediaId;
        private DispatcherQueue dispatcherQueue;
        private bool showSummary = true;

        public List<int> Series
        {
            get => _series;
            set => SetProperty(ref _series, value);
        }

        public int SegmentedSelectedIndex
        {
            get => segmentedSelectedIndex;
            set => SetProperty(ref segmentedSelectedIndex, value);
        }

        public bool ShowSummary
        {
            get => showSummary;
            set => SetProperty(ref showSummary, value);
        }

        public MediaModel Model
        {
            get => model;
            set => SetProperty(ref model, value);
        }

        public bool Loaded
        {
            get => loaded;
            set => SetProperty(ref loaded, value);
        }

        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public IAsyncRelayCommand LoadView { get; }

        public MediaViewViewModel(int mediaId, DispatcherQueue queue)
        {
            dispatcherQueue = queue;
            LoadView = new AsyncRelayCommand(InitView);
            MediaId = mediaId;
        }

        public async Task InitView()
        {
            await dispatcherQueue.EnqueueAsync(async () =>
            {
                Loaded = false;
                IsLoading = !Loaded;
                Model = await Initialize.FetchData(MediaId);
                Series = Model.Data.Media.Stats.ScoreDistribution.Select(
                    x => x.Amount
                ).ToList();
                Loaded = true;
                IsLoading = !Loaded;
            }, DispatcherQueuePriority.Normal);
        }

        public ICartesianAxis[] XAxes { get; set; } = new ICartesianAxis[]
        {
            new Axis
            {
                Labels = new string[] { "10", "20", "30", "40", "50", "60", "70", "80", "90", "100" },
                ForceStepToMin = true,
                MinStep = 10,
                ShowSeparatorLines = false,
                SeparatorsPaint = new SolidColorPaint(SKColors.Transparent)
            }
        };

        public ICartesianAxis[] YAxes { get; set; } = new ICartesianAxis[]
        {
            new Axis
            {
                LabelsPaint = new SolidColorPaint(SKColors.Transparent),
                ShowSeparatorLines = false,
                SeparatorsPaint = new SolidColorPaint(SKColors.Transparent)
            }
        };

        public void Expander_Expanding(Expander sender, ExpanderExpandingEventArgs args)
        {
            ShowSummary = false;
        }

        public void Expander_Collapsed(Expander sender, ExpanderCollapsedEventArgs args)
        {
            ShowSummary = true;
        }
    }
}
