using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace KGResourceTracking.Data;

public class KingdomGuardAPContext: DbContext
{
    public DbSet<Player> Players => Set<Player>();
    public DbSet<ApType> ApTypes => Set<ApType>();

    public KingdomGuardAPContext()
    {
        
    }
    public KingdomGuardAPContext(DbContextOptions<KingdomGuardAPContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=KGResourceTracking.db");
        base.OnConfiguring(optionsBuilder);
    }
    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    builder.Entity<ApType>().HasKey(t => new { t.Type, t.Id });
    //}

}
