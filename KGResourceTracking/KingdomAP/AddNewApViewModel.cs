using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using KGResourceTracking.Data;

using Microsoft.EntityFrameworkCore;

namespace KGResourceTracking.KingdomAP;

public partial class AddNewApViewModel : ObservableObject
{
    private readonly KingdomGuardAPContext DbContext;
    private readonly KingdomGuardPlayerDataServiceControllers _dBControler;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    private string _playerId = string.Empty;
    [ObservableProperty]
    private string _playerName = string.Empty;
    [ObservableProperty]
    private ApTypeEnum _apTypeChoice;
    [ObservableProperty]
    private Dictionary<ApTypeEnum, string> _apTypes = new Dictionary<ApTypeEnum, string>()
    {
        {ApTypeEnum.TwoHundred, "200 AP"},
        {ApTypeEnum.OneHunred, "100 AP"},
        {ApTypeEnum.Fifty, "50 AP"},
        {ApTypeEnum.Twenty, "20 AP"},
        {ApTypeEnum.Tenth, "10 AP"},
        {ApTypeEnum.Five, "5 AP"},
    };

    [ObservableProperty]
    private int _quality;
    public AddNewApViewModel(KingdomGuardAPContext dbContext, KingdomGuardPlayerDataServiceControllers dBControler)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dBControler = dBControler;
    }

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private async Task OnSubmit()
    {
        if (string.IsNullOrEmpty(PlayerId))
            return;
        ApType newAp = new()
        {
            Type = ApTypeChoice,
            Id = Int32.Parse(PlayerId),
            Count = Quality,
        };
        //Add player
        Player newPlayData = new Player() { Id = Int32.Parse(PlayerId), PlayerName = PlayerName };
        await _dBControler.AddPlayer(newPlayData);
        await _dBControler.SaveAndUpdateApForPlayer(newAp);
        await DbContext.SaveChangesAsync();
    }
    private bool CanSubmit() => !string.IsNullOrWhiteSpace(PlayerId);

}
