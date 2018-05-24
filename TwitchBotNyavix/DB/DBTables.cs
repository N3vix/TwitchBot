using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitchBotNyavix.DB
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UsId { get; set; }
        public string NickName { get; set; }
        public int Score { get; set; }
    }

    [Table("Commands")]
    public class Command
    {
        [Key]
        public int CmId { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string Actor { get; set; }
    }

}
