using AniMoe.App.Controls.UserViewControls.ViewModels;
using CommunityToolkit.Labs.WinUI;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AniMoe.App.Controls.UserViewControls
{
    public sealed partial class UserStatsPage : Page
    {
        public readonly UserStatViewModel ViewModel;

        public UserStatsPage()
        {
            this.InitializeComponent();
            ViewModel = new();
            DataContext = ViewModel;
        }
    }
}
