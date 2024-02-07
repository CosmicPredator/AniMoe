using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using AniMoe.Updater;
using AniMoe.App.Controls.SettingsViewControls;
using AniMoe.App.ViewModels;
using Microsoft.UI.Dispatching;

namespace AniMoe.App.Views
{
    public sealed partial class SettingsView : Page
    {
        public UpdateViewModel ViewModel;
        public SettingsView()
        {
            this.InitializeComponent();
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            ViewModel = new(this.XamlRoot, DispatcherQueue.GetForCurrentThread());
            DataContext = ViewModel;
        }
    }
}
