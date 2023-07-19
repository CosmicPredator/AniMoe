using AniMoe.App.Models.ReviewModel;
using AniMoe.Parsers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace AniMoe.App.ViewModels
{
    public partial class ReviewViewModel : ObservableObject
    {
        private ReviewModel model;
        private int reviewId;
        private WebView2 Webview;
        private IMdToHtmlParser MdToHtmlParser = App.Current.Services.GetRequiredService<IMdToHtmlParser>();
        private DispatcherQueue dispatcherQueue;
        private string createdTime;
        private MasterViewModel masterViewModel = App.Current.Services.GetRequiredService<MasterViewModel>();

        [ObservableProperty]
        private bool isLoading = true;

        [ObservableProperty]
        private bool loadEditButton = false;

        public ReviewModel Model
        {
            get => model;
            set => SetProperty(ref model, value);
        }

        public string CreatedTime
        {
            get => createdTime;
            set => SetProperty(ref createdTime, value);
        }

        public IAsyncRelayCommand LoadCommand { get; }

        private string EpochToString(long epochTime)
        {
            DateTime epochDateTime = DateTimeOffset.FromUnixTimeSeconds(epochTime).DateTime;
            TimeSpan timeDifference = DateTime.Now - epochDateTime;
            string relativeTime = GetRelativeTime(timeDifference);
            Console.WriteLine(relativeTime);
            return relativeTime;
        }

        private string GetRelativeTime(TimeSpan timeDifference)
        {
            if( timeDifference.TotalSeconds < 60 )
            {
                return "Just now";
            }
            else if( timeDifference.TotalMinutes < 60 )
            {
                int minutes = (int)Math.Floor(timeDifference.TotalMinutes);
                return $"{minutes} {(minutes == 1 ? "minute" : "minutes")} ago";
            }
            else if( timeDifference.TotalHours < 24 )
            {
                int hours = (int)Math.Floor(timeDifference.TotalHours);
                return $"{hours} {(hours == 1 ? "hour" : "hours")} ago";
            }
            else if( timeDifference.TotalDays < 365 )
            {
                int days = (int)Math.Floor(timeDifference.TotalDays);
                return $"{days} {(days == 1 ? "day" : "days")} ago";
            }
            else
            {
                int years = timeDifference.Days / 365;
                return $"{years} {(years == 1 ? "year" : "years")} ago";
            }
        }

        public ReviewViewModel(int reviewId, WebView2 webview, DispatcherQueue queue)
        {
            this.reviewId = reviewId;
            Webview = webview;
            dispatcherQueue = queue;
            LoadCommand = new AsyncRelayCommand(LoadView);
        }

        private async Task LoadView()
        {
            IsLoading = true;
            await dispatcherQueue.EnqueueAsync(async () =>
            {
                await Webview.EnsureCoreWebView2Async();
            }, DispatcherQueuePriority.High);
            Model = await Initialize.FetchData(reviewId);
            CreatedTime = EpochToString(Model.Data.Review.CreatedAt);
            if( Model.Data.Review.User.Id != masterViewModel.Model.Data.User.Id ) LoadEditButton = false;
            dispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
            {
                Webview.NavigateToString(MdToHtmlParser.Convert(Model.Data.Review.Body, true));
            });
            IsLoading = false;
        }
    }
}
