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
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using CommunityToolkit.WinUI;
using AniMoe.App.ViewModels;
using AniMoe.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace AniMoe.App.Views
{
    public sealed partial class CharacterView : Page
    {
        private DispatcherQueue dispatchQueue = DispatcherQueue.GetForCurrentThread();
        public CharacterViewModel ViewModel;
        public int CharId;
        public IMdToHtmlParser mdToHtmlParser = App.Current.Services.GetRequiredService<IMdToHtmlParser>();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CharId = 172759;
            ViewModel = new(CharId);
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }

        public CharacterView()
        {
            this.InitializeComponent();
            
        }

        private void SegmentedBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void MasterScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            await DescriptionWebView.EnsureCoreWebView2Async();
            await Task.Delay(500);
            DescriptionWebView.NavigateToString(
                mdToHtmlParser.Convert(ViewModel.Model.Data.Character.Description)
            );
        }

        private async void DescriptionWebView_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            string js = "Math.max(document.documentElement.scrollHeight, document.body.scrollHeight);";
            string heightString = await DescriptionWebView.ExecuteScriptAsync(js);

            if( double.TryParse(heightString, out double height) )
            {
                WebGrid.Height = height;
            }
        }
    }
}
