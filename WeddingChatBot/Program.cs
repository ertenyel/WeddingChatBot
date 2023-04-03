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
                            Users users;
                            if (chatUsers.GetUsers.Where(x => x.TelegramCode == message.From.Id).Count() == 0)
                            {
                                users = new Users
                                {
                                    TelegramCode = message.From.Id,
                                    Name = message.From.FirstName,
                                    Surname = message.From.LastName,
                                    IdChatPosition = "start"
                                };
                            }
                            else
                            {
                                users = chatUsers.GetUsers.Where(x => x.TelegramCode == message.From.Id).First();
                                users.IdChatPosition = "start";
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
        public static void BotAnswer(Message message, ITelegramBotClient botClient, ChatUsersContext chatUsers, Users user)
        {
            botClient.SendChatActionAsync(message.Chat, ChatAction.Typing);
            MessageTexts messageTexts = new MessageTexts(user.IdChatPosition, message);
            botClient.SendTextMessageAsync(message.Chat, messageTexts.Text, replyMarkup: messageTexts.Keyboard);
            AnswerKey chatAnswer = Answer.GetAnswerKey(message.Text);

            if (user.IdChatPosition == "start")
            {
                user.IdChatPosition = "responsetoinvite";
            }
            else if (user.IdChatPosition == "responsetoinvite")
            {
                if (chatAnswer == AnswerKey.Yes || chatAnswer == AnswerKey.MayBe)
                {
                    user.Choice = chatAnswer == AnswerKey.Yes ? "Согласен" : "Подумаю";
                    user.IdChatPosition = "otherpeople";
                }
                else if (chatAnswer == AnswerKey.No)
                {
                    user.Choice = "Не согласен";
                    user.IdChatPosition = "waitreason";
                }
            }
            else if (user.IdChatPosition == "otherpeople")
            {
                if (chatAnswer == AnswerKey.Yes)
                {
                    user.IdChatPosition = "selectalcohol";
                }
                else if (chatAnswer == AnswerKey.No)
                {
                    user.IdChatPosition = "writeotherpeople";
                }
            }
            else if (user.IdChatPosition == "waitreason")
            {
                user.IdChatPosition = "end";
            }
            else if (user.IdChatPosition == "selectalcohol")
            {
                user.Alcohol = message.Text;
                user.IdChatPosition = "end";
            }
            else if (user.IdChatPosition == "writeotherpeople")
            {
                user.PeopleGoTogether = message.Text;
                user.IdChatPosition = "selectalcohol";
            }
            else if (user.IdChatPosition == "end")
            {
                botClient.SendStickerAsync(message.Chat, new InputOnlineFile("CAACAgIAAxkBAAEICCdkBjw5uCHv7JPrVEYAAZHP4sxTgHUAAvcAA1advQoLciQdSPQNMC4E"));
                user.IdChatPosition = "end";
            }
            chatUsers.GetUsers.AddOrUpdate(user);
            chatUsers.SaveChanges();
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await Task.Run(() => { Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception)); });
        }


        static void Main(string[] args)
        {
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