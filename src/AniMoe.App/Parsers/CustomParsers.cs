using System.Text.RegularExpressions;
using System;
using Markdig;
using System.Diagnostics;

using AniMoe.Parsers;

public static class StringRegexes
{
    public static string ConvertSpoiler(this string htmlText)
    {
        string output = Regex.Replace(htmlText, 
            @"~!(.*?)!~", "<details><summary>Spoiler, Click to View</summary>$1</details>");
        return output;
    }

    public static string ConvertInlineLinks(this string htmlText)
    {
        string output = Regex.Replace(htmlText,
            @"\[(.*?)\]\((.*?)\)", "<a href=\"$2\" target=\"_blank\">$1</a>");
        return output;
    }

    public static string ConvertInlineImage(this string htmlText)
    {
        string output = Regex.Replace(htmlText,
            @"img(\d+)\((.*?)\)", "<img src=\"$2\" width=\"$1\"> <br>");
        return output;
    }

    public static string ConvertInlineImageWo(this string htmlText)
    {
        string output = Regex.Replace(htmlText,
            @"img\((.+?)\)", "<img src=\"$1\" width=\"100%\"> <br>");
        return output;
    }

    public static string ConvertInlineImagePercentage(this string htmlText)
    {
        string output = Regex.Replace(htmlText,
            @"img(\d+)%\((.+?)\)", "<img src=\"$2\" width=\"$1%\"> <br>");
        return output;
    }

    public static string ConvertYoutube(this string htmlText)
    {
        string output = Regex.Replace(htmlText,
            @"youtube\((https:\/\/youtu.be\/)(.*?)\)", @"<iframe width=""400"" height=""315"" src=""https://www.youtube.com/embed/$2"" title=""YouTube video player"" frameborder=""0"" allow=""accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"" allowfullscreen></iframe>");
        return output;
    }

    public static string ConvertWebm(this string htmlText)
    {
        string output = Regex.Replace(htmlText,
            @"webm\((.*?)\)", @"<video muted loop controls autoplay><source src=""$1"" type=""video/webm"">Your browser does not support the video tag.</video>");
        return output;
    }

    public static string ConvertCenter(this string htmlText)
    {
        string output = Regex
            .Replace(htmlText, @"~~~(.*?)~~~", @"<center>$1</center>");
        return output;
    }

    public static string ConvertInlineImageWLinkAttr(this string htmlText)
    {
        string output = Regex
            .Replace(htmlText, @"\[!\[.*?\]\((.*?)\)\]\((.*?)\)", 
            "<a href=\"$2\" rel=\"noopener noreferrer\" target=\"_blank\"><img src=\"$1\"></a> <br>");
        return output;
    }

    public static string ConvertBold(this string htmlText)
    {
        string output = Regex
            .Replace(htmlText, @"__(.*?)__", @"<b>$1</b>");
        return output;
    }

    public static string ConvertItalic(this string htmlText)
    {
        string output = Regex
            .Replace(htmlText, @"_(.*?)_", @"<i>$1</i>");
        return output;
    }

    public static string RemoveHtmlTags(this string htmlText)
    {
        if( htmlText == null ) return htmlText;
        string plainText = Regex.Replace(htmlText, @"<[^>]+>|&nbsp;", "").Trim();
        return plainText;
    }
}