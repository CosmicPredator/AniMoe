using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.Parsers
{
    public interface IMdToHtmlParser
    {
        string Convert(string rawHtml);
    }
}
