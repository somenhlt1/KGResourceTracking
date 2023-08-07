using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using KGResourceTracking.Data;

using MaterialDesignThemes.Wpf;

using Microsoft.EntityFrameworkCore;

namespace KGResourceTracking.KingdomAP;

public partial class AddNewApViewModel : ObservableObject
{
    private readonly KingdomGuardAPContext DbContext;
    private readonly KingdomGuardPlayerDataServiceControllers _dBControler;
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    [ObservableProperty]
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

    public ISnackbarMessageQueue MessagQueue { get; }

    public AddNewApViewModel(KingdomGuardAPContext dbContext, KingdomGuardPlayerDataServiceControllers dBControler, ISnackbarMessageQueue messagQueue)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dBControler = dBControler;
        MessagQueue = messagQueue;
    }

    [RelayCommand(CanExecute = nameof(CanSubmit),AllowConcurrentExecutions =false)]
    private async Task OnSubmit()
    {
        if (string.IsNullOrEmpty(PlayerId))
            return;
        ApType newAp = new()
        {
            Type = ApTypeChoice,
            Id = int.Parse(PlayerId),
            Count = Quality,
        };
        //Add player
        Player newPlayData = new Player() { Id = int.Parse(PlayerId), PlayerName = PlayerName };
        await _dBControler.AddPlayer(newPlayData);
        await _dBControler.SaveAndUpdateApForPlayer(newAp);
        await DbContext.SaveChangesAsync();
        MessagQueue.Enqueue("Saved!");
    }
    private bool CanSubmit() => !string.IsNullOrWhiteSpace(PlayerId);

}
