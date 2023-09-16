using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using AniMoe.App.Dialogs;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Hosting;

namespace AniMoe.App.Controls.UserViewControls
{
    public sealed partial class UserOverviewPage : Page
    {
        public UserOverviewPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ActivityEditorDialog dialog = new();
            dialog.XamlRoot = this.XamlRoot;
            await dialog.ShowAsync();
        }
    }
}
