﻿using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Converters
{
    public class ListToCommaSeperatedString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Join(", ", (value as List<string>));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
