using ABI.Windows.Media.Protection.PlayReady;
using Markdig;
using Microsoft.UI.Xaml;
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
        private string PreReq => $@"
        <style>
            html {{
              font-family: ""Segoe UI"", Arial, sans-serif;
              color: {(IsDarkMode ? "white" : "black")};
              overflow-x: hidden/clip;
            }}
            pre {{
                white-space: pre-wrap;
            }}
                
            *::-webkit-scrollbar {{
                width: 8px;
            }}

            *::-webkit-scrollbar-thumb {{
              background-color: #464646;
              border-radius: 20px;
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
                line-height: 0%;
            }}
        </style>";
        MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseAutoLinks()
                .Build();
        public string convert(string rawHtml)
        {
            rawHtml = rawHtml.Replace(@"\n\n", "")
                        .Replace(@"\n", "")
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
            return $"{PreReq}{Markdown.ToHtml(rawHtml, pipeline)}";
        }
    }
}
