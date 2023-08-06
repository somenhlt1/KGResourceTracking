using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using KGResourceTracking.Data;

using MaterialDesignColors;

using Microsoft.EntityFrameworkCore;

namespace KGResourceTracking.KingdomAP;

public partial class ViewPlayerApViewModel: ObservableObject
{
    private KingdomGuardAPContext KingdomGuardAPContext { get; }
    private KingdomGuardPlayerDataServiceControllers Controller { get; }
    public List<ViewPlayerApItemViewModel> ApTypes { get; set; } = new();
    public ObservableCollection<ViewPlayerApItemViewModel> ApTypesDataView { get;} = new();
    public ViewPlayerApViewModel(KingdomGuardAPContext kingdomGuardAPContext, KingdomGuardPlayerDataServiceControllers controller) 
    {
        KingdomGuardAPContext = kingdomGuardAPContext ?? throw new ArgumentNullException(nameof(kingdomGuardAPContext));
        Controller = controller;
        //TODO DO NOT DO THIS IN PRODUCTION
        //KingdomGuardAPContext.ApTypes.Load();
        BindingOperations.EnableCollectionSynchronization(ApTypes, new());
    }

    [ObservableProperty]
    private string _searchName;

    [RelayCommand]
    public async Task OnRefresh()
    {
        await LoadOutstandingApTypes();
    }
    [RelayCommand]
    public async Task OnRemove()
    {
        
        foreach (var item in ApTypes.Where(x=> x.IsSelected))
        {
          await Controller.RemovePlayer(new Player() { Id = item.PlayerId, PlayerName = item.PlayerName});

        }
        ApTypes = ApTypes.Where(x => !x.IsSelected).ToList();
        ApplySearch();
    }

    [ObservableProperty]
    private bool? _isAllPicked = false;
    public async Task LoadOutstandingApTypes()
    {
        ApTypes.Clear();
        await foreach (var player in KingdomGuardAPContext.Players.AsAsyncEnumerable())
        {
            ApTypes.Add(new ViewPlayerApItemViewModel(player.Id,player.PlayerName));
        }

        foreach (var player in ApTypes)
        {
            var query = from ap in KingdomGuardAPContext.ApTypes
                        where ap.Id == player.PlayerId
                        select new
                        {
                            ap.Type,
                            ap.Count,
                        };
            await foreach (var eachAp in query.AsAsyncEnumerable())
            {
                switch (eachAp.Type)
                {
                    case ApTypeEnum.TwoHundred:
                        player.Ap200 = eachAp.Count;
                        break;
                    case ApTypeEnum.OneHunred:
                        player.Ap100 = eachAp.Count;
                        break;
                    case ApTypeEnum.Fifty:
                        player.Ap50 = eachAp.Count;
                        break;
                    case ApTypeEnum.Twenty:
                        player.Ap20 = eachAp.Count;
                        break;
                    case ApTypeEnum.Tenth:
                        player.Ap10 = eachAp.Count;
                        break;
                    case ApTypeEnum.Five:
                        player.Ap5 = eachAp.Count;
                        break;
                    default:
                        break;
                }
            }
            player.PropertyChanged += (sender, e) =>
            { if (e.PropertyName == nameof(ViewPlayerApItemViewModel.IsSelected))
                {
                    var selected = ApTypes.Select(item => item.IsSelected).Distinct().ToList();
                    _isAllPicked = selected.Count == 1 ? selected.Single() : (bool?)null;
                    OnPropertyChanged(nameof(IsAllPicked));
                }
            };
        }
        ApplySearch();

    }

    private void ApplySearch() 
    {
        ApTypesDataView.Clear();
        
        foreach (var item in ApTypes.Where(x=> string.IsNullOrEmpty(SearchName) ? true : x.PlayerName.Contains(SearchName)))
        {
            ApTypesDataView.Add(item);
        }
    }

    partial void OnSearchNameChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            ApplySearch();
            Keyboard.ClearFocus();
        }
        else if (value.Contains("\r\n"))
        {
            _searchName = value.Replace("\r\n", "");
            ApplySearch();
            Keyboard.ClearFocus();
        }
        
    }
    partial void OnIsAllPickedChanged(bool? value)
    {
        SelectAll(value, ApTypes);
    }

    private void SelectAll(bool? select, IEnumerable<ViewPlayerApItemViewModel> models)
    {
        if (select.HasValue)
        {
            foreach (var model in models)
            {
                model.IsSelected = select.Value;
            }
        }
    }
}

public partial class ViewPlayerApItemViewModel : ObservableObject
{
    public int PlayerId { get;  }
    public string PlayerName { get; }
    [NotifyPropertyChangedFor(nameof(TotalAP))]
    [ObservableProperty]
    public int _ap200;
    [NotifyPropertyChangedFor(nameof(TotalAP))]
    [ObservableProperty]
    public int _ap100;
    [NotifyPropertyChangedFor(nameof(TotalAP))]
    [ObservableProperty]
    public int _ap50;
    [NotifyPropertyChangedFor(nameof(TotalAP))]
    [ObservableProperty]
    public int _ap20;
    [NotifyPropertyChangedFor(nameof(TotalAP))]
    [ObservableProperty]
    public int _ap10;
    [NotifyPropertyChangedFor(nameof(TotalAP))]
    [ObservableProperty]
    public int _ap5;
    [ObservableProperty]
    public bool _isSelected;
    public int TotalAP => TotalCount();


    public ViewPlayerApItemViewModel(int id,string name)
    {
        PlayerId = id;
        PlayerName = name;
        
    }

    private int TotalCount() 
    {
        return (Ap200*200) + (Ap100*100) + (Ap50*50) + (Ap20*20) + (Ap10*10) + (Ap5*5);
    }



}
