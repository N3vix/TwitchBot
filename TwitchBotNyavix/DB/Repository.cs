using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TwitchBotNyavix.DB
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        MessageContext _context;
        DbSet<TEntity> _dbSet;
        public Repository(MessageContext context)
        {
            _context = context; 
            _dbSet = context.Set<TEntity>();
        }
        public TEntity FindById(int id)
        {
            return _dbSet.Find(id);
        }
        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(predicate);
            return query;
        }
        public IEnumerable<TEntity> GetList()

        {
            return _dbSet.AsNoTracking().ToList();
        }
        public void Create(TEntity item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }
        public void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void Delete(TEntity item)
        {
            _dbSet.Remove(item);
            //  _context.SaveChanges();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            using (var db = new MessageContext())
            {
                return await _dbSet.FindAsync(id);
            }
        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            using (var db = new MessageContext())
            {
                return await _dbSet.ToListAsync();
            }
        }
        public async Task CreateAsync(TEntity item)
        {
            using (var db = new MessageContext())
            {
                _dbSet.Add(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync(TEntity item)
        {
            using (var db = new MessageContext())
            {
                _dbSet.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(TEntity item)
        {
            using (var db = new MessageContext())
            {
                _dbSet.Attach(item);
                _dbSet.Remove(item);
                await db.SaveChangesAsync();
            }
        }
    }
}
