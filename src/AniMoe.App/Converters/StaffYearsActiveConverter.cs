using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Converters
{
    public class StaffYearsActiveConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            List<int> ActiveYears = value as List<int>;
            if (ActiveYears.Count() == 2 )
            {
                return $"{ActiveYears.First()}-{ActiveYears.Last()}";
            }
            else
            {
                return $"{ActiveYears.First()}-Present";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
