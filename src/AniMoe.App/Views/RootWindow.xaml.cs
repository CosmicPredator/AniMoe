using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.Diagnostics;
using AniMoe.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using AniMoe.App.ViewModels;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.VoiceCommands;
using WinRT.Interop;

namespace AniMoe.App.Views
{ 
    public sealed partial class RootWindow : Window
    {
        #region Properties
        private readonly ILocalSettings _localSettings;

        public Thickness TitleBarThickness = new()
        {
            Left = 20,
            Top = 5
        };
        #endregion

        #region Constructor
        public RootWindow()
        {
            this.InitializeComponent();
            AppWindow.TitleBar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;
            if (MicaController.IsSupported())
                this.SystemBackdrop = new MicaBackdrop { Kind = MicaKind.BaseAlt };
            else 
                this.SystemBackdrop = new DesktopAcrylicBackdrop();
            SetAppWindowIcon();
            _localSettings = App.Current.Services.GetRequiredService<ILocalSettings>();
            AppTitleBar.Margin = TitleBarThickness;
            SetTitle();
            SetView();
            RootGrid.DataContext = this;
        }
        #endregion

        private void SetView()
        {
            if( _localSettings.IsSettingExists("accessToken") )
                PrimaryFrame.Navigate(typeof(MasterView));
            else
                PrimaryFrame.Navigate(typeof(LoginView));
        }

        private void SetTitle()
        { 
            SetTitleBar( AppTitleBar );
        }

        private void SetAppWindowIcon()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(id);
            appWindow.SetIcon("Assets/Window-Icon.ico");
        }

        public void ChangeTitleBarThickness(Thickness thickness)
        {
            AppTitleBar.Margin = thickness;
        }
    }
}
