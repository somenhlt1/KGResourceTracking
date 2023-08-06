using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGResourceTracking.Data
{
    public interface IKingdomGuardPlayerDataServiceControllers
    {
      public Task AddPlayer(Player player);
      public Task RemovePlayer(Player player);
      public Task SaveAndUpdateApForPlayer(ApType ap);
  
    }
}
