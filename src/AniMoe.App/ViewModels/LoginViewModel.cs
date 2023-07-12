using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AniMoe.App.Helpers;
using AniMoe.App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AniMoe.App.ViewModels
{
    public class LoginViewModel: ObservableRecipient
    {
        private readonly IResourceLocator locator = App
            .Current.Services.GetRequiredService<IResourceLocator>();
        public IAsyncRelayCommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand( async () => 
            {
                await Windows.System.Launcher.LaunchUriAsync(
                    new Uri(locator.GetAuthUrl()));
            });
        }
    }
}
