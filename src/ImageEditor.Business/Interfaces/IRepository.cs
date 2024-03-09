using ImageEditor.Business.Models;

namespace ImageEditor.Business.Interfaces;
public interface IRepository<TEntity> : IDisposable
    where TEntity : Entity
{
    public Task AddAsync(TEntity entity);
    public Task<TEntity> GetByIdAsync(Guid id);
    public Task<int> SaveChangesAsync();
}
