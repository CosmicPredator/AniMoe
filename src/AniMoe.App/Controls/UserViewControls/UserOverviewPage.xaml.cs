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
using System.Threading.Tasks;
using AniMoe.Parsers;
using Microsoft.Extensions.DependencyInjection;
using AniMoe.App.Models.UserModel;
using AniMoe.App.Models.UserActivityModel;

namespace AniMoe.App.Controls.UserViewControls
{
    public sealed partial class UserOverviewPage : Microsoft.UI.Xaml.Controls.Page
    {
        public UserActivityModel ActivityModel;

        public UserModel Model
        {
            get { return (UserModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }
        //private bool _loadGenreTable = false;
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(UserModel), typeof(UserOverviewPage), new PropertyMetadata(null));

        private IMdToHtmlParser mdToHtmlParser = App.Current.Services.GetRequiredService<IMdToHtmlParser>();
        public UserOverviewPage()
        {
            this.InitializeComponent();
            DataContext = this;
            RegisterPropertyChangedCallback(ModelProperty, ModelPropertyChanged);
            
        }

        private void ModelPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (Model.Data.User.Statistics.Anime.Genres.Any())
            {
                //_loadGenreTable = true;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ActivityEditorDialog dialog = new();
            dialog.XamlRoot = this.XamlRoot;
            await dialog.ShowAsync();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ActivityModel = await AniMoe.App.Models.UserActivityModel.Initialize.FetchData(851923);
            TestItRepeater.ItemsSource = ActivityModel.Data.Page.Activities;
        }
    }
}
