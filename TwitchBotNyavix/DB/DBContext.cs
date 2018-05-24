using System.Data.Entity;
using System.Linq;
using TwitchBotNyavix.Models;

namespace TwitchBotNyavix.DB
{
    class UserContext : DbContext
    {
        public UserContext() : base("DBConnection") { }
        public DbSet<User> Users { get; set; }

    //    #region Add user

    //    public void Add(string nickname, int password)
    //    {
    //        User user = new User { Nickname = nickname, Password = password };
    //        Users.Add(user);
    //        SaveChanges();
    //    }

    //    #endregion

    //    #region Delete user
    //    public void Delete(string nickname)
    //    {
    //        User p = Users.FirstOrDefault(x => x.Nickname == nickname);
    //        if (p != null)
    //        {
    //            Users.Remove(p);
    //            SaveChanges();
    //        }
    //    }
    //    #endregion

    //    #region Update user
    //    public void Update(string nickname, string newnickname)
    //    {
    //        User p = Users.FirstOrDefault(x => x.Nickname == nickname);
    //        if (p != null)
    //        {
    //            p.Nickname = newnickname;
    //            SaveChanges();
    //        }
    //    }
    //    public void Update(string nickname, int newpassword)
    //    {
    //        User p = Users.FirstOrDefault(x => x.Nickname == nickname);
    //        if (p != null)
    //        {
    //            p.Password = newpassword;
    //            SaveChanges();
    //        }
    //    }
    //    #endregion
    }

    public class MessageContext : DbContext
    {
        public MessageContext() : base("DBConnection") { }
        public DbSet<User> Users { get; set; }
        public DbSet<Command> Commands { get; set; }
    }
}
