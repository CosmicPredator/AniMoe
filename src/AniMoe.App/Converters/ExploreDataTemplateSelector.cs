using AniMoe.App.Models.CharacterExploreModel;
using AniMoe.App.Models.MediaExploreModel;
using AniMoe.App.Models.ReviewExploreModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AniMoe.App.Converters
{
    public class ExploreDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MediaTemplate { get; set; }
        public DataTemplate StaffCharTemplate { get; set; }
        public DataTemplate ReviewTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is AnimeManga )
            {
                return MediaTemplate;
            } else if (item is Character )
            {
                return StaffCharTemplate;
            } else if (item is Review )
            {
                return ReviewTemplate;
            }
            return null;
        }
    }
}
