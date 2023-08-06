using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Messaging;

using KGResourceTracking.Data;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel.Resolution;

using Moq.AutoMock.Resolvers;

namespace KGResourceTracking.Tests;

public static class AutoMockerExtensions
{
    public static IDbScope<KingdomGuardAPContext> WithDbScope(this AutoMocker mocker)
    {
        var resolver = new DbScopedResolver();
        var existing = mocker.Resolvers.ToList();
        mocker.Resolvers.Clear();
        existing.Insert(0, resolver);
        existing.Add(resolver);
        foreach (var existingResolver in existing)
        {
            mocker.Resolvers.Add(existingResolver);
        }
        return resolver;
    }
    public interface IDbScope<TContext> : IDbContextFactory<TContext>, IDisposable where TContext : DbContext
    {
        
    }
    private sealed class DbScopedResolver : IMockResolver, IDbScope<KingdomGuardAPContext>,IDisposable
    {
        private bool _disposedValue;

        public DbScopedResolver()
        {
            FilePath = Path.Combine(
                Path.GetTempPath(),
                "KGResourceTrackingTests",
                Guid.NewGuid().ToString("N")
                );
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
           

            using var context = CreateDbContext();
            context.Database.Migrate();
        }

        private string FilePath { get; }
        private IMessenger Messenger { get; }

        public void Resolve(MockResolutionContext context)
        {
            if (context.RequestType == typeof(KingdomGuardAPContext))
                context.Value = CreateDbContext();
            else if (context.RequestType == typeof(Func<KingdomGuardAPContext>))
            {
                context.Value = new Func<KingdomGuardAPContext>(CreateDbContext);
            }
        }



        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    File.Delete(FilePath);
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public KingdomGuardAPContext CreateDbContext()
        {
            var connectionString = new SqliteConnectionStringBuilder
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                DataSource = FilePath,
                Pooling = false
            };
            var options = new DbContextOptionsBuilder<KingdomGuardAPContext>().UseSqlite(connectionString.ToString()).Options;
            return new KingdomGuardAPContext(options);
        }
    }
}
