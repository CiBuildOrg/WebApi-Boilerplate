using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Entities.Contracts;

namespace App.Core.Contracts
{
    public interface IDbContext : IDisposable
    {
        void Attach<T>(T entity) where T : class, IBaseEntity;
        IQueryable<T> AsQueryable<T>() where T : class, IBaseEntity;
        void Update<T>(T entity) where T : class, IBaseEntity;
        void Save<T>(T entity) where T : class, IBaseEntity;

        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbEntityEntry Entry(object entity);
        DbSet Set(Type entityType);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}