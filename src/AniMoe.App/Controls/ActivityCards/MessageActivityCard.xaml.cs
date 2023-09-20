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
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AniMoe.App.Controls.ActivityCards
{
    public sealed partial class MessageActivityCard : UserControl, IDisposable
    {
        public MessageActivityCard()
        {
            this.InitializeComponent();
        }

        public void Dispose()
        {
            ActivityWebView.Close();
            GC.Collect();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await ActivityWebView.EnsureCoreWebView2Async();
        }
    }
}
