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
using CommunityToolkit.Labs.WinUI;
using AniMoe.App.Dialogs.ActivityEditorDialogControls;

namespace AniMoe.App.Dialogs
{
    public sealed partial class ActivityEditorDialog : ContentDialog
    {
        private readonly EditorPage editorPage;
        private readonly PreviewPage previewPage;
        public ActivityEditorDialog()
        {
            this.InitializeComponent();
            editorPage = new();
            previewPage = new();
            selectionSegmented.SelectedIndex = 0;
        }

        private void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Segmented seg = (Segmented)sender;
            previewPage.SetPreviewContent(editorPage.GetActivityMessage());
            if (ContentFrame.Children.Count != 0) ContentFrame.Children.Clear();
            if (seg.SelectedIndex == 0)
                ContentFrame.Children.Add(editorPage);
            else
                ContentFrame.Children.Add(previewPage);
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            previewPage.DisposeWebView();
        }
    }
}
