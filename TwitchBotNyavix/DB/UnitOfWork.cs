using System;
using System.Threading.Tasks;

namespace TwitchBotNyavix.DB
{
    public class UnitOfWork : IDisposable
    {
        private MessageContext db = new MessageContext();

        private Repository<User> UsRepository;
        private Repository<Command> CmRepository;
        private bool disposed = false;

        public Repository<User> Users => UsRepository ?? (UsRepository = new Repository<User>(db));
        public Repository<Command> Commands => CmRepository ?? (CmRepository = new Repository<Command>(db));
        public void Save()
        {
            db.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}