using AniMoe.App.Models.CharacterModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.ViewModels
{
    public partial class CharacterViewModel : ObservableObject
    {
        private int characterId;

        [ObservableProperty]
        private CharacterModel model;

        [ObservableProperty]
        private int selectedIndex;

        public IAsyncRelayCommand LoadView { get; }

        public CharacterViewModel(int charId)
        {
            LoadView = new AsyncRelayCommand(LoadApi);
            characterId = charId;
        }

        private async Task LoadApi()
        {
            Model = await Initialize.FetchData(characterId);
        }
    }
}
