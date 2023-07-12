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
using AniMoe.App.Models.MediaModel;

namespace AniMoe.App.Controls
{
    public sealed partial class MediaSideViewControl : UserControl
    {
        public MediaModel Model
        {
            get { return (MediaModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(MediaModel), 
                typeof(MediaSideViewControl), new PropertyMetadata(null));

        public MediaSideViewControl()
        {
            this.InitializeComponent();
        }
    }
}
