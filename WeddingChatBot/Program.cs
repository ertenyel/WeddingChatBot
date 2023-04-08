﻿using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
//using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeddingChatBot;
using WeddingChatBot.DataModel;

namespace TelegramBotExperiments
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ChatUsersContext chatUsers = new ChatUsersContext())
            {
                chatUsers.GetUsers.Where(x => x.TelegramCode == 1).Count();
            }
            var bot = new TelegramBotClient("6119932961:AAFQ2-JsIOnAS7-khwx8Chhu8JolUiiYnnU");
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cancellationToken = new CancellationTokenSource().Token;
            var receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var datemes = DateTime.Now - (update.Message.Date.AddHours(3));
            if (update.Type == UpdateType.Message && datemes.Seconds < 5)
            {
                var message = update.Message;
                if (!string.IsNullOrWhiteSpace(message.Text))
                {
                    using (ChatUsersContext chatUsers = new ChatUsersContext())
                    {
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
                            if (message.Text.ToLower() == "/start")
                            {
                                Console.WriteLine($"{message.From.FirstName} {message.From.LastName}: {message.Text}; {message.Date}");
                                users.IdChatPosition = mainPosId;
                            }
                            else if (chatUsers.GetButtons.Where(x => x.Text == message.Text).Count() > 0)
                            {
                                int? newIdChatPosition = chatUsers.GetButtons.Where(x => x.Text == message.Text).Select(x => x.CalledChatPosition).First();
                                int? buttonType = chatUsers.GetButtons.Where(x => x.Text == message.Text).Select(x => x.ButtonType).First();
                                if (buttonType == 0)
                                {
                                    users.IdChatPosition = (int)newIdChatPosition;
                                }
                                else if (buttonType == 4 || buttonType == 1)
                                {
                                    if (buttonType == 1)
                                    {
                                        users.Choice = message.Text;
                                    }
                                    users.IdChatPosition = (int)chatUsers.GetChatPositions.Where(x => x.Id == users.IdChatPosition).Select(x => x.ParentId).First();
                                }
                                else if (buttonType == 5)
                                {
                                    users.IdChatPosition = mainPosId;
                                }
                                else if (buttonType == 2)
                                {
                                    users.Alcohol = users.Alcohol + ", " + message.From.Id;
                                }
                                else if (buttonType == 3)
                                {
                                    users.Food = users.Food + ", " + message.From.Id;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }

                        chatUsers.GetUsers.AddOrUpdate(users);
                        chatUsers.SaveChanges();
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
            IReplyMarkup replyKeyboardMarkups = Keyboard.Get(chatUsers, user);

            botClient.SendTextMessageAsync(message.Chat, messageText, replyMarkup: replyKeyboardMarkups);

            chatUsers.GetUsers.AddOrUpdate(user);
            chatUsers.SaveChanges();
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await Task.Run(() => { Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception)); });
        }
    }
}