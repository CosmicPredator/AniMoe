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
using AniMoe.App.ViewModels;
using AniMoe.App.Models.CharacterModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using AniMoe.App.Views;
using Microsoft.UI.Xaml.Media.Animation;
using CommunityToolkit.WinUI.UI.Controls;

namespace AniMoe.App.Controls.CharacterViewControls
{
    public sealed partial class AnimeListControl : Page
    {
        public Media ViewModel;
        public string MediaType;
        public List<string> VALanguages = new();
        public ObservableCollection<Edge> SelectedChars = new();
        public Frame Rootframe;
        public bool IsVAComboBoxVisible;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var data = (Dictionary<string, dynamic>)e.Parameter;
            ViewModel = data["ViewModel"] as Media;
            MediaType = data["Type"] as string;
            Rootframe = data["RootFrame"] as Frame;
            DataContext = this;
            IsVAComboBoxVisible = MediaType == "ANIME";
            base.OnNavigatedTo(e);
        }

        public AnimeListControl()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (MediaType == "ANIME" )
            {
                foreach( var i in ViewModel.Edges )
                {
                    if( i.Node.Type == MediaType )
                    {
                        foreach( var j in i.VoiceActors )
                        {
                            if( !VALanguages.Contains(j.LanguageV2) )
                            {
                                VALanguages.Add(j.LanguageV2);
                            }
                        }
                    }
                }
                LanguageComboBox.ItemsSource = VALanguages;
                LanguageComboBox.SelectedIndex = 0;
            } else
            {
                SelectedChars.Clear();
                foreach( var i in ViewModel.Edges )
                {
                    if( i.Node.Type == MediaType )
                    {
                        SelectedChars.Add(i);
                    }
                }
                CharacterListItRepeater.ItemsSource = SelectedChars;
            }
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedChars.Clear();
            foreach( var i in ViewModel.Edges )
            {
                foreach( var j in i.VoiceActors )
                {
                    if( j.LanguageV2 == LanguageComboBox.SelectedValue.ToString() )
                    {
                        i.SelectedVoiceActor = j;
                        SelectedChars.Add(i);
                    }
                }
            }

            CharacterListItRepeater.ItemsSource = SelectedChars;
        }

        private void CharacterImage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ImageEx g = sender as ImageEx;
            if( e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse )
            {
                var properties = e.GetCurrentPoint(this).Properties;
                if( properties.IsLeftButtonPressed )
                {
                    Rootframe.Navigate(typeof(MediaView), (int)g.Tag, new DrillInNavigationTransitionInfo());
                }
            }
        }
    }
}
