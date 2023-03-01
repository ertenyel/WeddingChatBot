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

namespace TelegramBotExperiments
{
    class Program
    {
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                using (ChatUsersContext chatUsers = new ChatUsersContext())
                {
                    if (message.Text.ToLower() == "/start")
                    {
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

                    await Task.Run(() => { BotAnswer(update, botClient, chatUsers, user); });
                }
            }
        }
        public static void BotAnswer(Update update, ITelegramBotClient botClient, ChatUsersContext chatUsers, Users user)
        {
            var message = update.Message;
            Console.WriteLine($"{message.From.FirstName} {message.From.LastName}: {message.Text}; {message.Date}");
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                if (user.IdChatPosition == "start")
                {
                    IEnumerable<KeyboardButton> keyboardButtons = new List<KeyboardButton>()
                    {
                        new KeyboardButton("Точно да!"),
                        new KeyboardButton("Да кому вы нужны!"),
                        new KeyboardButton("Ну посмотрим...")
                    };
                    var rkm = new ReplyKeyboardMarkup(keyboardButtons);
                    botClient.SendTextMessageAsync(message.Chat,
                        "Привет!\n\nПриглашаем Вас на нашу свадьбу 09.06.2023!!!\n\nВсё будет происходить в ресторане метелица!\n\nВы идете?",
                        replyMarkup: rkm);
                    user.IdChatPosition = "responsetoinvite";
                }
                else if (user.IdChatPosition == "responsetoinvite")
                {
                    if (message.Text == "Точно да!")
                    {
                        IEnumerable<KeyboardButton> keyboardButtons = new List<KeyboardButton>()
                        {
                            new KeyboardButton("Да"),
                            new KeyboardButton("Нет"),
                        };
                        var rkm = new ReplyKeyboardMarkup(keyboardButtons);
                        botClient.SendTextMessageAsync(message.Chat, "Запишу Ваш выбор карандашом! Будете один?", replyMarkup: rkm);
                        user.Choice = "Согласен";
                        user.IdChatPosition = "otherpeople";
                    }
                    else if (message.Text == "Да кому вы нужны!")
                    {
                        botClient.SendTextMessageAsync(message.Chat, "Это еще почему?", replyMarkup: new ReplyKeyboardRemove());
                        user.Choice = "Не согласен";
                        user.Alcohol = user.PeopleGoTogether = null;
                        user.IdChatPosition = "waitreason";
                    }
                }
                else if (user.IdChatPosition == "otherpeople")
                {
                    if (message.Text == "Да")
                    {
                        botClient.SendTextMessageAsync(message.Chat, "Понял, что хотели бы выпивать?", replyMarkup: new ReplyKeyboardRemove());
                        user.IdChatPosition = "selectalcohol";
                    }
                    else if (message.Text == "Нет")
                    {
                        botClient.SendTextMessageAsync(message.Chat, "Напишите имена и фамилии с кем вы будете через запятую.", replyMarkup: new ReplyKeyboardRemove());
                        user.IdChatPosition = "writeotherpeople";
                    }
                }
                else if (user.IdChatPosition == "waitreason")
                {
                    botClient.SendTextMessageAsync(message.Chat, "Я конечно записал ваш ответ, но даю всё-таки минуту подумать. Меня можно перезагрузить и заполнить еще раз...", replyMarkup: new ReplyKeyboardRemove());
                    user.IdChatPosition = "end";
                }
                else if (user.IdChatPosition == "selectalcohol")
                {
                    botClient.SendTextMessageAsync(message.Chat, "Ну всё записал! Ждем!", replyMarkup: new ReplyKeyboardRemove());
                    user.Alcohol = message.Text;
                    user.IdChatPosition = "end";
                }
                else if (user.IdChatPosition == "writeotherpeople")
                {
                    botClient.SendTextMessageAsync(message.Chat, "Всё четко.\nчто хотели бы выпивать?", replyMarkup: new ReplyKeyboardRemove());
                    user.PeopleGoTogether = message.Text;
                    user.IdChatPosition = "selectalcohol";
                }
                else if (user.IdChatPosition == "end")
                {
                    botClient.SendTextMessageAsync(message.Chat, "Ваш ответ я заполнил. Если вы хотите поговорить, то я не тот ассистент", replyMarkup: new ReplyKeyboardRemove());
                    user.IdChatPosition = "end";
                }
                chatUsers.GetUsers.AddOrUpdate(user);
                chatUsers.SaveChanges();
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
            //await Task.Run(() => { Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception)); });
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