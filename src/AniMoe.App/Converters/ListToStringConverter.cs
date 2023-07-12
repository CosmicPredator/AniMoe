using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using AniMoe.App.Models.MediaModel;
using System.Linq;
using System.Xml.Linq;

namespace AniMoe.App.Converters
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is List<StudioNode>)
            {
                List<StudioNode> Nodes = value as List<StudioNode>;
                List<string> list = Nodes.Select(x => x.Name).ToList();
                if( list.Count == 0 ) return "?";
                else return string.Join(", ", list);
            } else if (value is List<string>) { }
            {
                List<string> list = value as List<string>;
                if( list.Count == 0 ) return "?";
                else return string.Join(", ", list);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
