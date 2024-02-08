using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Management.Deployment;

namespace AniMoe.Updater
{
    public class UpdateService
    {
        public async Task UpdateAniMoe(Uri assetUrl)
        {
            uint res = ReopenHandler.RegisterApplicationRestart(null, ReopenHandler.RestartFlags.NONE);

            PackageManager pm = new();
            var data = await pm.UpdatePackageAsync(
                assetUrl,
                null,
                DeploymentOptions.ForceApplicationShutdown
            );
            Debug.WriteLine(data.IsRegistered);
        }
    }
}
