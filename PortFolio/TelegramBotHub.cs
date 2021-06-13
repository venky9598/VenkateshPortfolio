using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;

namespace PortFolio
{
    public class TelegramBotHub : Hub
    {
        static ConcurrentDictionary<string, List<int>> concurrentDictionary =
                    new ConcurrentDictionary<string, List<int>>();

        static ITelegramBotClient botClient = 
                    new TelegramBotClient("879673655:AAG-4VHByqoG04ykQgH8t-9VOKlwTjWJfpw");

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            List<int> values = new List<int>();

            if (concurrentDictionary.ContainsKey(Context.ConnectionId))
            {
                var ids = concurrentDictionary[Context.ConnectionId];

                foreach(var id in ids)
                {
                    SendMessage(id);
                }

                concurrentDictionary.TryRemove(Context.ConnectionId, out values);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public void JoinUser(List<int> userIds)
        {
            if(!concurrentDictionary.ContainsKey(Context.ConnectionId))
            {
                concurrentDictionary.TryAdd(Context.ConnectionId, userIds);

                var ids = concurrentDictionary[Context.ConnectionId];

                foreach (var id in ids)
                {
                    SendMessageOnline(id);
                }
            }
        }

        public async void SendMessage(int id)
        {
           await botClient.SendTextMessageAsync(
            chatId: id,
            text: DecodeEncodedNonAsciiCharacters2("Your Machine is Offline \U0001F61E")
          );
        }

        public async void SendMessageOnline(int id)
        {
            await botClient.SendTextMessageAsync(
             chatId: id,
             text: DecodeEncodedNonAsciiCharacters2("Your Machine is Online \U0001F601")
           );
        }

        public string DecodeEncodedNonAsciiCharacters2(string value)
        {
            return Regex.Replace(
            value,
            @"\\u(?<Value>[a-zA-Z0-9]{4})",
            m =>
            {
                return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
            });
        }
    }
}
