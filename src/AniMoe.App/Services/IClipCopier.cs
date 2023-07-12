using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Services
{
    public interface IClipCopier
    {
        void CopyToClipboard(string text);
    }
}
