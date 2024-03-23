using AniMoe.App.Controls.SettingsViewControls;
using AniMoe.Updater;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Semver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace AniMoe.App.ViewModels
{
    public partial class UpdateViewModel : ObservableObject
    {

        private UpdateHandler handler = App.Current.Services.GetRequiredService<UpdateHandler>();
        private XamlRoot _currentXamlRoot;
        private DispatcherQueue _dispatcherQueue;

        [ObservableProperty]
        private string _currentAppVersion;

        [ObservableProperty]
        private GithubModel _model;

        [ObservableProperty]
        private string _buttonStatus = "Check for Update";

        [ObservableProperty]
        private bool _loaderState;

        [ObservableProperty]
        private bool _updateButtonEnable = true;

        [RelayCommand]
        private async Task runRequest()
        {
            LoaderState = true;
            //Model = await handler.CheckLatestRelease();
            LoaderState = false;
            var appVersion = SemVersion.Parse(CurrentAppVersion, SemVersionStyles.Any);
            var newVersion = SemVersion.Parse(Model.TagName, SemVersionStyles.Any);
            if (SemVersion.ComparePrecedence(newVersion, appVersion) > 0)
            {
                _dispatcherQueue.TryEnqueue(async () =>
                {
                    UpdateDialog dialog = new(Model);
                    dialog.XamlRoot = null;
                    dialog.XamlRoot = _currentXamlRoot;
                    await dialog.ShowAsync();
                });
            } else
            {
                ButtonStatus = "No Updates Available";
                UpdateButtonEnable = false;
            }
        }

        public UpdateViewModel(XamlRoot xamlRoot, DispatcherQueue dispatcherQueue)
        {
            CurrentAppVersion = string.Format("{0}.{1}.{2}",
                    Package.Current.Id.Version.Major,
                    Package.Current.Id.Version.Minor,
                    Package.Current.Id.Version.Build);
            _currentXamlRoot = xamlRoot;
            _dispatcherQueue = dispatcherQueue;
        }
    }
}
