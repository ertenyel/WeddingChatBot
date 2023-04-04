using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
//using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq;
using System.Collections.Generic;
using WeddingChatBot.DataModel;
using System.Data.Entity.Migrations;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Telegram.Bot.Types.Enums;
using WeddingChatBot;
using Telegram.Bot.Types.InputFiles;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace TelegramBotExperiments
{
    class Program
    {
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                if (!string.IsNullOrWhiteSpace(message.Text))
                {
                    using (ChatUsersContext chatUsers = new ChatUsersContext())
                    {
                        if (message.Text.ToLower() == "/start")
                        {
                            Console.WriteLine($"{message.From.FirstName} {message.From.LastName}: {message.Text}; {message.Date}");
                            WeddingChatBot.DataModel.User users;
                            int mainPosId = chatUsers.GetChatPositions.Where(x => x.Name == "main").Select(x => x.Id).First();

                            if (chatUsers.GetUsers.Where(x => x.TelegramCode == message.From.Id).Count() == 0)
                            {
                                users = new WeddingChatBot.DataModel.User
                                {
                                    TelegramCode = message.From.Id,
                                    Name = message.From.FirstName,
                                    Surname = message.From.LastName,
                                    IdChatPosition = mainPosId
                                };
                            }
                            else
                            {
                                users = chatUsers.GetUsers.Where(x => x.TelegramCode == message.From.Id).First();
                                users.IdChatPosition = mainPosId;
                            }
                            chatUsers.GetUsers.AddOrUpdate(users);
                            chatUsers.SaveChanges();
                        }
                        var user = chatUsers.GetUsers.Where(x => x.TelegramCode == message.From.Id).First();

                        await Task.Run(() => { BotAnswer(message, botClient, chatUsers, user); });
                    }
                }
            }
        }
        public static void BotAnswer(Message message, ITelegramBotClient botClient, ChatUsersContext chatUsers, WeddingChatBot.DataModel.User user)
        {
            int? idMessage = chatUsers.GetChatPositions.Where(x => x.Id == user.IdChatPosition).Select(x => x.IdMessageText).First();
            string messageText = chatUsers.GetTextsInMessage.Where(x => x.Id == idMessage).Select(x => x.Text).First();
            int? idButtons = chatUsers.GetTextsInMessage.Where(x => x.Id == idMessage).Select(x => x.IdButtons).First();
            List<string> buttons = chatUsers.GetButtons.Where(x => x.IdMessage == idButtons).Select(x => x.Text).ToList();

            IReplyMarkup replyKeyboardMarkups;
            if (buttons.Count > 0)
            {
                List<KeyboardButton> keyboardButtons = new List<KeyboardButton>();

                foreach (string button in buttons)
                    keyboardButtons.Add(new KeyboardButton(button));

                replyKeyboardMarkups = new ReplyKeyboardMarkup(keyboardButtons);
            }
            else
            {
                replyKeyboardMarkups = new ReplyKeyboardRemove();
            }
            botClient.SendTextMessageAsync(message.Chat, messageText, replyMarkup: replyKeyboardMarkups);

            chatUsers.GetUsers.AddOrUpdate(user);
            chatUsers.SaveChanges();
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await Task.Run(() => { Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception)); });
        }


        static void Main(string[] args)
        {
            //using (ChatUsersContext chatUsers = new ChatUsersContext())
            //{
            //    chatUsers.GetUsers.Where(x => x.TelegramCode == 1).Count();
            //}
            TelegramBotClient bot = new TelegramBotClient("6119932961:AAFQ2-JsIOnAS7-khwx8Chhu8JolUiiYnnU");
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}