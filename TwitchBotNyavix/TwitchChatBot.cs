using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwitchBotNyavix.DB;
using TwitchBotNyavix.Models;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;
using TwitchLib.Events.Services.LiveStreamMonitor;
using TwitchLib.Models.API.Undocumented.Comments;
using TwitchLib.Models.API.v3.Streams;
using TwitchLib.Models.API.v3.Videos;
using TwitchLib.Models.API.v5.Users;
using User = TwitchBotNyavix.DB.User;

namespace TwitchBotNyavix
{
    internal class TwitchChatBot
    {
        UnitOfWork Userdb = new UnitOfWork();
        List<User> UserList;
        List<Command> CommandsList;

        OnStreamOnlineArgs live = new OnStreamOnlineArgs();
        readonly ConnectionCredentials credentials = new ConnectionCredentials(TwitchInfo.BotUserName, TwitchInfo.BotToken);
        TwitchClient client;

        List<Votes> votes = new List<Votes>();
        internal void Connect()
        {
            Console.WriteLine("Connection");
            UserList = Userdb.Users.GetList().ToList();
            CommandsList = Userdb.Commands.GetList().ToList();
            client = new TwitchClient(credentials, TwitchInfo.ChannelName, logging: true);

            client.OnLog += Client_Onlog;
            client.OnConnectionError += Client_OnConnectionError;
            client.OnMessageReceived += Clien_OnMessageReceiver;
            client.OnUserJoined += Client_OnUserJoined;
            client.Connect();
        }

        private void Client_OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            if (UserList.Equals())
            {
                UserList.Add(new User { NickName = e.Username, Score = 0 });
                Userdb.Users.Update(new User { NickName = e.Username, Score = 0 });
            }
        }

        internal void Disconnect()
        {
            client.Disconnect();
            Console.WriteLine("Disconnecting");
        }

        private void Clien_OnMessageReceiver(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.DisplayName == "N3vix")
            {
                if (e.ChatMessage.Message.StartsWith("!create", StringComparison.InvariantCultureIgnoreCase))
                {
                    string[] lines = e.ChatMessage.Message.Split(' ', '\t');
                    string message = "";
                    for (int i = 2; i < lines.Length - 1; i++)
                    {
                        message += lines[i] + " ";
                    }

                    CommandsList.Add(new Command { Name = lines[1], Message = message, Actor = lines.Last() });
                    Userdb.Commands.Update(new Command { Name = lines[1], Message = message, Actor = lines.Last() });

                    client.SendMessage($"Comm created, {e.ChatMessage.DisplayName}");
                }
                if (e.ChatMessage.Message.StartsWith("!votecreate", StringComparison.InvariantCultureIgnoreCase))
                {
                    string[] lines1 = e.ChatMessage.Message.Split(',');
                    string[] lines = lines1[0].Split(' ', '\t');
                    List<Vote> list = new List<Vote>();
                    for (int i = 1; i < lines1.Length; i++)
                    {
                        list.Add(new Vote{Name = lines1[i], Votes = 0});
                    }
                    votes.Add(new Votes { Name = lines[1], List = list});
                    client.SendMessage($"Vote created, {e.ChatMessage.DisplayName}");
                    foreach (Votes vote in votes)
                    {
                        if (vote.Name == lines[1])
                        {
                            foreach (Vote vote1 in vote.List)
                            {
                                client.SendMessage($"Vote : {vote1.Name}");
                            }
                        }
                    }
                }
            }
            if (e.ChatMessage.Message.StartsWith("!"))
            {
                if (e.ChatMessage.Message.StartsWith("!vote", StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach (Votes vote in votes)
                    {
                        if (e.ChatMessage.Message.Contains(vote.Name))
                        {
                            foreach (Vote vote1 in vote.List)
                            {
                                if (e.ChatMessage.Message.Contains(vote1.Name))
                                {
                                    vote1.Votes++;
                                }
                            }
                        }
                    }
                }
                if (e.ChatMessage.Message.StartsWith("!result", StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach (Votes vote in votes)
                    {
                        if (e.ChatMessage.Message.Contains(vote.Name))
                        {
                            foreach (Vote vote1 in vote.List)
                            {
                                client.SendMessage($"{vote1.Name} : votes {vote1.Votes}");
                            }
                        }
                        else
                        {
                            client.SendMessage("Enter correct vote name");
                        }
                    }
                }
                if (!e.ChatMessage.Message.StartsWith("!vote", StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach (Command comm in CommandsList)
                    {
                        if (e.ChatMessage.Message.StartsWith("!" + comm.Name,StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (comm.Actor == "none")
                            {
                                client.SendMessage($"{comm.Message}");
                            }
                            if (comm.Actor == "self")
                            {
                                client.SendMessage($"{comm.Message} ,{e.ChatMessage.DisplayName}");
                            }
                            if (comm.Actor == "streamer")
                            {
                                client.SendMessage($"{comm.Message} ,{e.ChatMessage.Channel}");
                            }
                            if (comm.Actor == "viewer")
                            {
                                string[] lines = e.ChatMessage.Message.Split(' ', '\t');
                                client.SendMessage($"{comm.Message} ,{lines[1]}");
                            }
                        }
                    }
                }
                if (e.ChatMessage.Message.StartsWith("!roulette", StringComparison.InvariantCultureIgnoreCase))
                {
                }
                if (e.ChatMessage.Message.StartsWith("!uptime", StringComparison.InvariantCultureIgnoreCase))
                {
                    client.SendMessage($"Height , {live.Stream.Channel.CreatedAt}");
                }
            }
        }

        private void Client_Onlog(object sender, OnLogArgs e)
        {
            //Console.WriteLine(e.Data);
        }
        private void Client_OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"Error!  {e.Error}");
        }

    }
}