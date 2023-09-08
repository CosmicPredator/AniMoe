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

namespace AniMoe.App.Views
{ 
    public sealed partial class RootWindow : Window
    {
        #region Properties
        private ILocalSettings _localSettings;

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
            LoadIcon("Assets/Window-Icon.ico");
            _localSettings = App.Current.Services.GetService<ILocalSettings>();
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

        private void LoadIcon(string iconName)
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            IntPtr hIcon = PInvoke.User32.LoadImage(IntPtr.Zero, iconName,
                      PInvoke.User32.ImageType.IMAGE_ICON, 16, 16, PInvoke.User32.LoadImageFlags.LR_LOADFROMFILE);

            PInvoke.User32.SendMessage(hwnd, PInvoke.User32.WindowMessage.WM_SETICON, (IntPtr)0, hIcon);
        }

        public void ChangeTitleBarThickness(Thickness thickness)
        {
            AppTitleBar.Margin = thickness;
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("AD056AB3-1FBB-4F8D-963D-7ED80A181C2D")]
        internal interface IWindowNative
        {
            IntPtr WindowHandle { get; }
        }
    }
}
