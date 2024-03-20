using AniMoe.App.Models.MasterModel;
using AniMoe.App.Models.MediaListStatusModel;
using AniMoe.App.Services;
using AniMoe.App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AniMoe.App.ViewModels
{
    public partial class SplashViewModel : ObservableObject
    {
        public MediaListStatusModel mediaListStatusModel;
        private readonly ILocalSettings _localSettings;
        private RootWindow rootWindow = null;

        [ObservableProperty] private MasterModel model;

        [ObservableProperty] string _statusText;

        public SplashViewModel(MasterModel model, ILocalSettings settings)
        {
            _localSettings = settings;
            StatusText = "Registering Dependencies...";
            model = Model;
        }

        public async void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            var ins = Application.Current as App;
            var window = ins.m_window;
            if (window is SplashView splashView) return;
            if (window is RootWindow roowindow) return;
            if (_localSettings.IsSettingExists("accessToken"))
                await LoadFromApi();
            if (rootWindow == null)
            {
                var mwindow = new RootWindow();
                mwindow.ExtendsContentIntoTitleBar = true;
                ((Window)sender).Close();
                mwindow.Activate();
                ins.m_window = mwindow;
            }
        }

        public async Task LoadFromApi()
        {
            StatusText = "Fetching user data...";
            Model = await Models.MasterModel.Initialize.FetchData();

            StatusText = "Fetching Anime & Manga lists...";
            mediaListStatusModel
                = await Models.MediaListStatusModel.Initialize.FetchData(Model.Data.User.Id);
            EnumValue defaultValue = new EnumValue { Name = "Select One" };
            Model.Data.MediaSourceList.EnumValues.Insert(0, defaultValue);
            Model.Data.MediaSeasonList.EnumValues.Insert(0, defaultValue);
            Model.Data.MediaFormatList.EnumValues.Insert(0, defaultValue);

            StatusText = "Launching AniMoe...";
        }
    }
}
