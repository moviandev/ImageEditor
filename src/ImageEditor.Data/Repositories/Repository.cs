using System;
using System.Threading.Tasks;
using ImageEditor.Business.Interfaces;
using ImageEditor.Business.Models;
using ImageEditor.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ImageEditor.Data.Repositories;
public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly ImageEditorContext Db;
    protected readonly DbSet<TEntity> DbSet;

    protected Repository(ImageEditorContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public virtual async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await DbSet.FirstOrDefaultAsync(d => d.Id == id);
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        DbSet.Add(entity);
        await SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        await SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Db.SaveChangesAsync();
    }

    public void Dispose()
    {
        Db?.Dispose();
    }
}
