using AniMoe.App.Models.StaffCharListModel;
using AniMoe.App.Models.StaffModel;
using CommunityToolkit.Common.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AniMoe.App.ViewModels
{
    public partial class StaffViewModel : ObservableObject
    {
        private int StaffId;

        [ObservableProperty]
        private StaffModel model;
        public IAsyncRelayCommand LoadData { get; }

        public StaffViewModel(int staffId, ScrollViewer parentScrollView, ItemsRepeater itemsRepeater)
        {
            LoadData = new AsyncRelayCommand(InitView);
            StaffId = staffId;
        }

        private async Task InitView()
        {
            Model = await Models.StaffModel.Initialize.FetchData(StaffId);
        }
    }

    public class CharacterListIncrementalList: IncrementalLoadingCollection<CharacterListLoader, Edge>
    {
        private readonly CharacterListLoader Loader = new();

        public CharacterListIncrementalList(int StaffId, bool onList = false)
        {
            Loader.StaffId = StaffId;
            Loader.OnList = onList;
        }

        protected override Task<IEnumerable<Edge>> LoadDataAsync(CancellationToken cancellationToken)
        {
            return Loader.GetPagedItemsAsync(1, 1);
        }
    }

    public class CharacterListLoader : IIncrementalSource<Edge>
    {
        private bool HasNextPage = true;
        private int PageNum = 1;
        public int StaffId;
        public bool OnList;

        public async Task<IEnumerable<Edge>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            if( HasNextPage )
            {
                StaffCharListModel Model =
                    await Models.StaffCharListModel.Initialize.FetchData(StaffId, OnList, PageNum);
                HasNextPage = Model.Data.Staff.CharacterMedia.PageInfo.HasNextPage;
                PageNum++;
                foreach( var item in Model.Data.Staff.CharacterMedia.Edges )
                {
                    if( item.Characters.Count() >= 1 )
                    {
                        item.SelectedCharacter = item.Characters[0];
                    }
                }
                return Model.Data.Staff.CharacterMedia.Edges;
            }
            else return new List<Edge>();
        }
    }
}
