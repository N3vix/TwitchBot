using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TwitchBotNyavix.DB
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Create(TEntity item);
        TEntity FindById(int id);
        IEnumerable<TEntity> GetList();
        void Delete(TEntity item);
        void Update(TEntity item);
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        Task CreateAsync(TEntity item);
        Task UpdateAsync(TEntity item);
        Task DeleteAsync(TEntity item);
    }
}
