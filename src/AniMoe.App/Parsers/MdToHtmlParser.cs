using ABI.Windows.Media.Protection.PlayReady;
using Markdig;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.Parsers
{
    public class MdToHtmlParser : IMdToHtmlParser
    {
        private bool IsDarkMode { get; set; }
        public MdToHtmlParser()
        {
            IsDarkMode = Application.Current.RequestedTheme != ApplicationTheme.Light;
        }

        private Tuple<byte, byte, byte, byte> ConvertAccent()
        {
            var accentColorResource = Application.Current.Resources["SystemAccentColorLight2"];
            var accentColor = (Windows.UI.Color)accentColorResource;
            return Tuple.Create(accentColor.R, accentColor.G, accentColor.B, accentColor.A);
        }

        private Tuple<byte, byte, byte, byte> ConvertAccent1()
        {
            var accentColorResource = Application.Current.Resources["ControlStrongFillColorDefaultBrush"];
            var accentColor = (SolidColorBrush)accentColorResource;
            return Tuple.Create(accentColor.Color.R, accentColor.Color.G, accentColor.Color.B, accentColor.Color.A);
        }

        MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseAutoLinks()
                .Build();
        public string Convert(string rawHtml, bool isReview = false)
        {
            string PreReq = $@"
                <style>
                    html {{
                      font-family: ""Segoe UI"", Arial, sans-serif;
                      font-size: 1.2em;
                      color: {(IsDarkMode ? "white" : "black")};
                      overflow-x: hidden/clip;
                      {(isReview? "overflow: hidden;": "overflow: auto;")}
                      {(isReview ? @"padding-top: 50px;
                      padding-right: 100px;
                      padding-bottom: 50px;
                      padding-left: 100px;": "")}
                    }}
                    pre {{
                        white-space: pre-wrap;
                    }}

                    a:link {{
                      color: rgb({ConvertAccent().Item1}, {ConvertAccent().Item2}, {ConvertAccent().Item3}, {ConvertAccent().Item4});
                      background-color: transparent;
                      text-decoration: none;
                    }}

                    a:hover {{
                      color: rgb({ConvertAccent().Item1}, {ConvertAccent().Item2}, {ConvertAccent().Item3}, {ConvertAccent().Item4});
                      background-color: transparent;
                      text-decoration: underline;
                    }}

                    a:visited {{
                      color: rgb({ConvertAccent().Item1}, {ConvertAccent().Item2}, {ConvertAccent().Item3}, {ConvertAccent().Item4});
                      background-color: transparent;
                      text-decoration: none;
                    }}

                    p {{
                      white-space: pre-wrap;
                      word-wrap: break-word;
                      overflow-wrap: break-word;
                      line-height: 140%
                    }}

                    blockquote {{
                      border-left: 10px solid #ccc;
                      margin: 1.5em 10px;
                      padding: 0.5em 10px;
                    }}
                    blockquote:before {{
                      color: #ccc;
                      font-size: 4em;
                      line-height: 0.1em;
                      margin-right: 0.25em;
                      vertical-align: -0.4em;
                    }}
                    blockquote p {{
                      display: inline;
                      font-style: italic
                    }}

                    p > code,
                    li > code,
                    dd > code,
                    td > code {{
                      background: #002342;
                      word-wrap: break-word;
                      box-decoration-break: clone;
                      padding: .1rem .3rem .2rem;
                      border-radius: .2rem;
                    }}
                    code {{
                        font-family: 'JetBrains Mono';
                    }}
                    video {{
                        max-width: 100%;
                        float:left;
                    }}
                    img {{
                        max-width: 100%;
                        float:left;
                    }}

                    br {{
                        line-height: 20px;
                    }}
                </style>";
            //rawHtml = Markdown.ToHtml(rawHtml, pipeline);
        rawHtml = rawHtml
                        .Replace(@"\n", "<br>")
                        .Replace(@"\n\n", "<br>")
                        .Replace(@"\""", "'")
                        .ConvertSpoiler()
                        .ConvertInlineImage()
                        .ConvertYoutube()
                        .ConvertWebm()
                        .ConvertCenter()
                        .ConvertInlineImagePercentage()
                        .ConvertInlineImageWo()
                        .ConvertBold()
                        .ConvertInlineImageWLinkAttr()
                        .ConvertInlineLinks()
                        .ConvertItalic();
            return $@"{PreReq}<div id='mainDiv'> {(Markdown.ToHtml(rawHtml, pipeline))} </div>";
        }
    }
}
