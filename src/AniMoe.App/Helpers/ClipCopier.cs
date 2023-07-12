using AniMoe.App.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace AniMoe.App.Helpers
{
    public class ClipCopier : IClipCopier
    {
        private DataPackage dataPackage = new DataPackage();
        public void CopyToClipboard(string text)
        {
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);
            Debug.WriteLine("ClipBoard Content Set...!");
        }
    }
}
