using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KGResourceTracking.Data;

public class KingdomGuardPlayerDataServiceControllers : IKingdomGuardPlayerDataServiceControllers
{
    public KingdomGuardAPContext DBContext { get; }

    public KingdomGuardPlayerDataServiceControllers(KingdomGuardAPContext dBContext)
    {
        DBContext = dBContext;
    }

    public async Task AddPlayer(Player player)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player));
        var existedPlayer = await DBContext.Players.FindAsync(player.Id);
        if (existedPlayer is { })
        {
            existedPlayer.PlayerName = player.PlayerName;
            DBContext.Players.Update(existedPlayer);
        }
        else
        {
            DBContext.Players.Add(player);
        }
        
    }

    public async Task RemovePlayer(Player player)
    {
        if (player is null)
            return;
        try
        {
            var existedPlayer = await DBContext.Players.FindAsync(player.Id);
            if (existedPlayer is { })
            {
                DBContext.Players.Remove(existedPlayer);
            }
            await DBContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            
        }
       
    }

    public async Task SaveAndUpdateApForPlayer(ApType ap)
    {
        if (ap is null)
            return;
       
        var existingAp = await DBContext.ApTypes.FindAsync(ap.Id,ap.Type);
        if (existingAp is { } && ap.Count != existingAp.Count)
        {
            ap.Count = ap.Count;
            DBContext.ApTypes.Update(existingAp);   
           
        }
        else
        {
          DBContext.ApTypes.Add(ap);
         
        }

    }
}
