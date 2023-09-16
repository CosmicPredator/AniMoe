using AniMoe.Parsers;
using Microsoft.Extensions.DependencyInjection;
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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AniMoe.App.Dialogs.ActivityEditorDialogControls
{
    public sealed partial class PreviewPage : UserControl
    {
        private IMdToHtmlParser mdToHtmlParser = 
            App.Current.Services.GetRequiredService<IMdToHtmlParser>();

        public PreviewPage()
        {
            this.InitializeComponent();
            _ = PreviewWebView.EnsureCoreWebView2Async();
        }

        public async void SetPreviewContent(string activityMessage)
        {
            await Task.Delay(1000);
            PreviewWebView.NavigateToString(mdToHtmlParser.Convert(activityMessage));
        }

        public void DisposeWebView()
        {
            PreviewWebView.Close();
            GC.Collect();
        }
    }
}
