using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PortFolio.Models;
using Telegram.Bot;
using Telegram.Bot.Args;
using Microsoft.CognitiveServices.Speech;
using VisioForge.Shared.NAudio.Wave;

namespace PortFolio.Controllers
{
    public class HomeController : Controller
    {
        static ITelegramBotClient botClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> StartBot()
        {
            botClient = new TelegramBotClient("1844622929:AAFdyx8WNHw_nRQuKHntSjTsHXKdB1beh3Q");

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;

            botClient.StartReceiving();
            return Json("Running");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                string message = string.Empty;

                await SynthesizeAudioAsync(e.Message.Text);

                string currentDir = System.IO.Directory.GetCurrentDirectory() + @"\MyFile.wav";

                using (var stream = System.IO.File.OpenRead(currentDir))
                {
                    await botClient.SendAudioAsync(
                      chatId: e.Message.Chat,
                      audio: stream,
                      duration: 10,
                      performer: "textttovoicebot",
                      title: Guid.NewGuid().ToString() + ".wav"
                    );
                }

                System.IO.File.Delete(currentDir);
            }
        }

        static async Task SynthesizeAudioAsync(string message)
        {
            string currentDir = System.IO.Directory.GetCurrentDirectory() + @"\MyFile.wav";
            var config = SpeechConfig.FromSubscription("8fedc56eeae3450c8fedd6eaf05e16a1", "southeastasia");
            config.SpeechSynthesisLanguage = "hi-IN";
            config.SpeechSynthesisVoiceName = "hi-IN-SwaraNeural";
            using var synthesizer = new SpeechSynthesizer(config, null);
            var result = await synthesizer.SpeakTextAsync(message);
            using var stream = AudioDataStream.FromResult(result);
            await stream.SaveToWaveFileAsync(currentDir);
        }
    }
}
