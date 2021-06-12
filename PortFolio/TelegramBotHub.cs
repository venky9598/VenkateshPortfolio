using Microsoft.AspNetCore.SignalR;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace PortFolio
{
    public class TelegramBotHub : Hub
    {
        static bool isOnline = false;

        static ITelegramBotClient botClient = new TelegramBotClient("879673655:AAG-4VHByqoG04ykQgH8t-9VOKlwTjWJfpw");

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            isOnline = false;
            while (!isOnline)
            {
                SendMessage(621742107);
                SendMessage(805484468);
                Thread.Sleep(1000 * 60 * 5);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public override async Task OnConnectedAsync()
        {
            isOnline = true;
            SendMessageOnline(621742107);
            SendMessageOnline(805484468);

            await base.OnConnectedAsync();
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
