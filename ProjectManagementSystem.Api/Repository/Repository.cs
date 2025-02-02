﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectManagementSystem.Api.Data;
using ProjectManagementSystem.Api.Entities;
using System.Linq.Expressions;

namespace ProjectManagementSystem.Api.Repository;
public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly AppDbContext _appDbContext;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _dbSet = _appDbContext.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity)
    {
        entity.CreatedAt = DateTime.Now;

        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.Now;

        }
        await _dbSet.AddRangeAsync(entities);
    }

    public void Delete(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        SaveInclude(entity, p => p.IsDeleted, p => p.DeletedAt);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            Delete(entity);
        }
    }


    public void Undelete(TEntity entity)
    {
        entity.IsDeleted = false;
        entity.DeletedAt = null;
        SaveInclude(entity, p => p.IsDeleted, p => p.DeletedAt);
    }

    public void HardDelete(TEntity entity) => _dbSet.Remove(entity);
    public IQueryable<TEntity> GetAll() => _dbSet.Where(e => !e.IsDeleted);
    public IQueryable<TEntity> AsQuerable() => _dbSet.Where(e => !e.IsDeleted);

    public IQueryable<TEntity> GetAllWithDeleted() => _dbSet;
    public async Task<TEntity?> GetByIdAsync(int id) => await GetAll().FirstOrDefaultAsync(x => x.Id == id);
    public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression) => GetAll().Where(expression);

    public void SaveInclude(TEntity entity, params Expression<Func<TEntity, object>>[] propertyExpressions)
    {
        var local = _dbSet.Local.FirstOrDefault(x => x.Id == entity.Id);

        EntityEntry<TEntity> entityEntry = null;

        if (local is null)
            entityEntry = _appDbContext.Entry(entity);
        else
            entityEntry = _appDbContext.ChangeTracker.Entries<TEntity>()
                .First(x => x.Entity.Id == entity.Id);

        foreach (var propertyExpression in propertyExpressions)
        {
            var propertyToUpdate = entityEntry.Property(propertyExpression);
            var propertyName = propertyToUpdate.Metadata.Name;
            propertyToUpdate.CurrentValue = entity.GetType().GetProperty(propertyName)?.GetValue(entity);
            propertyToUpdate.IsModified = true;
        }
    }

    public void SaveExclude(TEntity entity, params Expression<Func<TEntity, object>>[] propertyExpressions)
    {
        var propertiesToExclude = new HashSet<string>(propertyExpressions.Select(e => ((MemberExpression)e.Body).Member.Name));

        var local = _dbSet.Local.FirstOrDefault(i => i.Id == entity.Id);
        EntityEntry<TEntity> entityEntry = null;
        if (local is null)
            entityEntry = _appDbContext.Entry(entity);
        else
            entityEntry = _appDbContext.ChangeTracker.Entries<TEntity>().First(i => i.Entity.Id == entity.Id);

        foreach (var propertyEntry in entityEntry.Properties)
        {
            var propertyName = propertyEntry.Metadata.Name;

            if (propertiesToExclude.Contains(propertyName))
                propertyEntry.IsModified = false;
            else
                propertyEntry.IsModified = true;
        }
    }
    public void ClearedTrackedChanges(TEntity entity)
    {
        var local = _dbSet.Local.FirstOrDefault(e => e.Id == entity.Id);

        if (local is not null)
        {
            _dbSet.Entry(local).State = EntityState.Detached;
        }



    }
    public void SaveIncludeRange(IEnumerable<TEntity> entities, params Expression<Func<TEntity, object>>[] propertyExpressions)
    {
        foreach (var entity in entities)
        {
            SaveInclude(entity, propertyExpressions);
        }
    }

    public async Task<bool> DoesEntityExistAsync(int id) => await _dbSet.AnyAsync(e => e.Id == id);

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }

    public void UpdateFullEntity(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);


    }

    public IQueryable<TEntity> GetAllWithInclude(Func<IQueryable<TEntity>, IQueryable<TEntity>> expression)
    {
        return expression(_dbSet);
    }
}
