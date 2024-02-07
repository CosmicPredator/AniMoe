using AniMoe.Updater;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AniMoe.App.Controls.SettingsViewControls
{
    public sealed partial class UpdateDialog : ContentDialog
    {
        public GithubModel Model;
        public UpdateDialog(GithubModel model)
        {
            this.InitializeComponent();
            Model = model;
            DataContext = this;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            foreach(var i in Model.Assets)
            {
                if (i.Name.Contains(".msix"))
                {
                    Debug.WriteLine(i.Name);
                    Debug.WriteLine(i.Size);
                    Debug.WriteLine(i.BrowserDownloadUrl);
                }
            }
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            double assetSize = Math.Round(
                (Model.Assets.First(x => x.Name.Contains(".msix")).Size * 0.000001), 2);
            SizeTextBlock.Text = $"{assetSize}MB";
        }

        private async void MdTextBlock_LinkClicked(object sender, CommunityToolkit.WinUI.UI.Controls.LinkClickedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));
        }
    }
}
