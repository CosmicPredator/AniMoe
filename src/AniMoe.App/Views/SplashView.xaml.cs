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
using Windows.Win32;
using WinRT.Interop;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Foundation;
using AniMoe.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AniMoe.App.Views
{
    public sealed partial class SplashView : Window
    {
        SplashViewModel ViewModel = App.Current.Services.GetRequiredService<SplashViewModel>();

        public SplashView()
        {
            this.InitializeComponent();
            convertToSplashWindow();
        }

        void convertToSplashWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);

            var screenWidth = AniMoeAppNative.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXSCREEN);
            var screenHeight = AniMoeAppNative.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CYSCREEN);

            var desiredWidth = 640;
            var desiredHeight = 360;

            unchecked
            {
                AniMoeAppNative.SetWindowLongPtr((HWND)hWnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE, (nint)WINDOW_STYLE.WS_POPUPWINDOW);
            }

            AniMoeAppNative.SetWindowPos((HWND)hWnd,
                (HWND)new IntPtr(0),
                (screenWidth / 2) - (desiredWidth / 2),
                (screenHeight / 2) - (desiredHeight / 2),
                desiredWidth, desiredHeight,
                SET_WINDOW_POS_FLAGS.SWP_NOZORDER);
        }
    }
}
