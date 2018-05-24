using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBotNyavix
{
    class Program
    {
        static void Main(string[] args)
        {
            TwitchChatBot bot = new TwitchChatBot();

            bot.Connect();

            Console.ReadLine();
        }
    }
}
