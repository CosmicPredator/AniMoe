using AniMoe.App.Models.AnimeListModels;
using AniMoe.App.Models.MangaListModel;
using AniMoe.App.Models.MasterModel;
using AniMoe.App.Models.MediaListStatusModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.ViewModels
{
    public partial class MasterViewModel : ObservableRecipient
    {
        private MasterModel model;
        private bool navItemsEnable = true;
        private bool isVisible = false;
        public MediaListStatusModel mediaListStatusModel;

        public MasterModel Model
        {
            get => model;
            set => SetProperty(ref model, value);
        }

        public bool NavItemsEnable
        {
            get => navItemsEnable;
            set => SetProperty(ref navItemsEnable, value);
        }

        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }

        public IAsyncRelayCommand LoadData { get; }

        public MasterViewModel()
        {
            LoadData = new AsyncRelayCommand(LoadModel);
        }

        private async Task LoadModel()
        {
            IsVisible = false;
            NavItemsEnable = true;
            Model = await Models.MasterModel.Initialize.FetchData();
            mediaListStatusModel 
                = await Models.MediaListStatusModel.Initialize.FetchData(Model.Data.User.Id);
            EnumValue defaultValue = new EnumValue { Name = "Select One" };
            Model.Data.MediaSourceList.EnumValues.Insert(0, defaultValue);
            Model.Data.MediaSeasonList.EnumValues.Insert(0, defaultValue);
            Model.Data.MediaFormatList.EnumValues.Insert(0, defaultValue);
            NavItemsEnable = false;
            IsVisible = true;
        }
    }
}
