using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Management.Deployment;

namespace AniMoe.Updater
{
    public class Entry
    {
        public async Task UpdateAniMoe()
        {
            uint res = ReopenHandler.RegisterApplicationRestart(null, ReopenHandler.RestartFlags.NONE);

            PackageManager pm = new();
            var data = await pm.UpdatePackageAsync(
                new Uri("https://i-will-do.with-your.mom/r/AniMoe.App_0.0.2.102_x64.msix"),
                null,
                DeploymentOptions.ForceApplicationShutdown
            );
            Debug.WriteLine(data.IsRegistered);
        }
    }
}
