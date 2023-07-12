using AniMoe.App.Models.CharacterExploreModel;
using AniMoe.App.Models.MediaExploreModel;
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
        public DataTemplate Template1 { get; set; }
        public DataTemplate Template2 { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is AnimeManga )
            {
                return Template1;
            } else if (item is Character )
            {
                return Template2;
            }
            return null;
        }
    }
}
