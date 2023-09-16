using AniMoe.Parsers;
using Microsoft.Extensions.DependencyInjection;
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

namespace AniMoe.App.Views
{
    public sealed partial class UserView : Page
    {
        private readonly IMdToHtmlParser mdToHtmlParser;

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            AboutWebView.Close();
            GC.Collect();
        }

        public UserView()
        {
            this.InitializeComponent();
            mdToHtmlParser = App.Current.Services.GetRequiredService<IMdToHtmlParser>();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await AboutWebView.EnsureCoreWebView2Async();
            AboutWebView.NavigateToString(mdToHtmlParser.Convert(AboutHtml));
        }

        private string AboutHtml = @"[](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)\n\n~~~webm(https://i.imgur.com/CiVuVKn.mp4)~~~ \n\n~~~img(https://i.imgur.com/FHlaVpR.png) [ img(https://i.imgur.com/euWafr3.png) ](https://anilist.co/forum/thread/6062/1)~~~\n\n~~~img(https://i.imgur.com/5iUbUYB.png)~~~ \n\n\n\n~~~webm(https://files.catbox.moe/oh08qu.webm)~~~\n&nbsp;\n~~~img(https://i.imgur.com/5fPYOb8.gif)~~~\n\n~~~  ──.⋅.⋅ [_`polar`_](http://polar.tumblr.com/) ✘ [_`soundcloud`_](https://soundcloud.com/kusanagi) ✘ [_`waifulist`_](https://mywaifulist.moe/u/asuna) ⋅.⋅.──~~~ \n\n\n~~~[_`mal`_](https://myanimelist.net/profile/Akira) ✘ [_`steam`_](https://steamcommunity.com/id/kosuna) ✘ [_`spotify`_](https://open.spotify.com/playlist/33ulk5kZtwBQg9pKv4E01a?si=VLBcUgApQvCiNU8_poOWEw)  ~~~ \n\n~~~___`\""ᴘᴜʀꜱᴜɪᴛ ᴏꜰ ʜᴀᴘᴘɪɴᴇꜱꜱ\"" `___~~~\n\n~~~<a>___`元気出して !`___</a>~~~ <div align=\""center\""> [⁸²⁴](https://youtu.be/7wkEYJB5aoM)</div>\n\n~~~img(https://i.imgur.com/z6nkfxo.png) ~~~\n\n[img(https://i.imgur.com/tFs4G4Q.png) ](https://youtu.be/EhTH8OIPoY4)\n\nimg(https://i.imgur.com/ZvnmsJ5.png) img(https://i.imgur.com/kkTAiAN.png) [img(https://i.imgur.com/BWuH4XY.png) ](https://anilist.co/activity/62787243) \n\nimg(https://i.imgur.com/MVXDq1u.png)\n\n\n\n~~~img(https://i.imgur.com/ScxKoyn.png)~~~\n\n[img(https://i.imgur.com/pvh9nvU.png) ](https://i.imgur.com/pvh9nvU.png)\n[img(https://i.imgur.com/L1StGB9.png) ](https://i.imgur.com/L1StGB9.png)\n[img(https://i.imgur.com/4AiOSWr.png) ](https://i.imgur.com/4AiOSWr.png)\n[img(https://i.imgur.com/wzAtaGK.png) ](https://i.imgur.com/wzAtaGK.png)\n[img(https://i.imgur.com/Ur5dL16.png) ](https://i.imgur.com/Ur5dL16.png)\n[img(https://i.imgur.com/rLi0tH2.png) ](https://i.imgur.com/rLi0tH2.png)\n~~~[ img300(https://i.imgur.com/aaWSK9Q.png) ](https://imgur.com/a/ycWK8cX)~~~\n\n~~~[ img(https://i.imgur.com/TeYZfQk.png) ](https://youtu.be/mfMl83MOjPs)~~~ ~~~img(https://i.imgur.com/9RyBeAY.png)~~~\n\n\n\n~~~[ img50(https://i.imgur.com/9r6i9Gm.gif) ](https://youtu.be/C5i-UnuUKUI)~~~\n\n<div align=\""right\"">[img40(https://i.imgur.com/LJAqvbl.png) ](https://youtu.be/2EpBqtqf8Go)\n&nbsp;";
    }
}
