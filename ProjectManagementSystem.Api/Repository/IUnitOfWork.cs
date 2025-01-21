using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Repository;
public interface IUnitOfWork
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
    Task<int> SaveChangesAsync();
}
