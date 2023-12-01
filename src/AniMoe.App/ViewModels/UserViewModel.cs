using AniMoe.App.Models.UserModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.ViewModels
{
    public partial class UserViewModel : ObservableObject
    {
        private int _userId;

        [ObservableProperty]
        private UserModel _model;

        [RelayCommand]
        private async Task GetUserDetails()
        {
            Model = await Initialize.FetchData(_userId, false);
        }

        public UserViewModel(int userId, bool isSelf)
        {
            _userId = userId;
        }
    }
}
