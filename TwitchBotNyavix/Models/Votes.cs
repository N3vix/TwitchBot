using System;
using System.Collections.Generic;

namespace TwitchBotNyavix.Models
{
    class Votes
    {
        public string Name { get; set; }
        public List<Vote> List { get; set; }
    }

    class Vote
    {
        public string Name { get; set; }
        public int Votes { get; set; }
    }
}
