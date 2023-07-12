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

namespace AniMoe.App.Views
{ 
    public sealed partial class RootWindow : Window
    {
        private ILocalSettings _localSettings;
        public RootWindow()
        {
            this.InitializeComponent();
            LoadIcon("Assets/Window-Icon.ico");
            _localSettings = App.Current.Services.GetService<ILocalSettings>();
            SetTitle();
            SetView();
        }

        private void SetView()
        {
            if( _localSettings.IsSettingExists("accessToken") ) 
                RootFrame.Navigate(typeof(MasterView));
            else 
                RootFrame.Navigate(typeof(LoginView));
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

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("AD056AB3-1FBB-4F8D-963D-7ED80A181C2D")]
        internal interface IWindowNative
        {
            IntPtr WindowHandle { get; }
        }
    }
}
