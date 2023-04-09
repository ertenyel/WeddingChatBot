using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
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
            var bot = new TelegramBotClient("5864890772:AAE5AEVouqCRX6AyBbo26PzPHiJwT9tUmfA");
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
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                var datemes = DateTime.Now - (message.Date.AddHours(3));
                if (!string.IsNullOrWhiteSpace(message.Text) && datemes.Seconds < 5)
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
                            else if (message.Text.ToLower() == "/clear")
                            {
                                users.Companion = users.Alcohol = users.Choice = users.Food = null;
                            }
                            else if (chatUsers.GetButtons.Where(x => x.Text == message.Text).Count() > 0)
                            {
                                var button = chatUsers.GetButtons.Where(x => x.Text == message.Text).Select(x => new { x.CalledChatPosition, x.ButtonType }).First();

                                if (button.ButtonType == 0)
                                {
                                    users.IdChatPosition = (int)button.CalledChatPosition;
                                }
                                else if (button.ButtonType == 4 || button.ButtonType == 1 || users.IdChatPosition == 9)
                                {
                                    if (button.ButtonType == 1)
                                    {
                                        users.Choice = message.Text;
                                        string stickerId;
                                        if (message.Text == "С удовольствием буду!✅")
                                            stickerId = "CAACAgIAAxkBAAEIgbNkMWRFsx0B7YYl5uo2CYIsvujvbwACBAEAAladvQreBNF6Zmb3bC8E";
                                        else
                                            stickerId = "CAACAgIAAxkBAAEIgbFkMWRD_bmCyDcIcVIAAQyCpPkewjMAAvsAA1advQpWDtsz28rJ5i8E";

                                        await botClient.SendStickerAsync(message.Chat, new Telegram.Bot.Types.InputFiles.InputOnlineFile(stickerId));
                                    }
                                    users.IdChatPosition = (int)chatUsers.GetChatPositions.Where(x => x.Id == users.IdChatPosition).Select(x => x.ParentId).First();
                                }
                                else if (button.ButtonType == 5)
                                {
                                    users.IdChatPosition = mainPosId;
                                }
                                else if (button.ButtonType == 2)
                                {
                                    users.Alcohol = users.Alcohol + ", " + message.Text;
                                }
                                else if (button.ButtonType == 3)
                                {
                                    users.Food = users.Food + ", " + message.Text;
                                }
                            }
                            else if (users.IdChatPosition == 9)
                            {
                                users.Companion = message.Text;
                                users.IdChatPosition = (int)chatUsers.GetChatPositions.Where(x => x.Id == users.IdChatPosition).Select(x => x.ParentId).First();
                                await botClient.SendStickerAsync(message.Chat, new Telegram.Bot.Types.InputFiles.InputOnlineFile("CAACAgIAAxkBAAEIhIZkMpDO0FWA9M5OJtbwf2IuuCWXDQAC_gADVp29CtoEYTAu-df_LwQ"));
                            }
                            else
                            {
                                await botClient.SendStickerAsync(message.Chat, new Telegram.Bot.Types.InputFiles.InputOnlineFile("CAACAgIAAxkBAAEIhIpkMpEvvyN7LkBroSNctShjS8ylLAAC-QADVp29CpVlbqsqKxs2LwQ"));
                                return;
                            }
                        }

                        if (users.IdChatPosition == 3
                                && !string.IsNullOrWhiteSpace(users.Companion) && !string.IsNullOrWhiteSpace(users.Alcohol) && !string.IsNullOrWhiteSpace(users.Choice) && !string.IsNullOrWhiteSpace(users.Food))
                        {
                            await botClient.SendTextMessageAsync(message.Chat, $"Поздравляем, {users.Name}!\nУ вас заполнена вся анкета!\nЕсли вы хотите что-нибудь изменить, то используйте меню бота снова.");
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
            var textEntry = chatUsers.GetTextsInMessage.Where(x => x.Id == idMessage).Select(x => new 
            { 
                x.Text, 
                x.ImageUrl, 
                x.FirstLatitude, 
                x.FirstLongitude, 
                x.SecondLatitude, 
                x.SecondLongitude}).First();

            IReplyMarkup replyKeyboardMarkups = Keyboard.Get(chatUsers, user);
            botClient.SendTextMessageAsync(message.Chat, textEntry.Text, replyMarkup: replyKeyboardMarkups);

            if (textEntry.ImageUrl != null)
            {
                Thread.Sleep(500);
                using (Media photos = new Media())
                {
                    List<InputMediaPhoto> album = photos.GetAlbumPhotos(textEntry.ImageUrl);

                    if (album != null)
                        botClient.SendMediaGroupAsync(message.Chat, album);
                    
                    Thread.Sleep(1000);
                }
            }
            if (textEntry.FirstLatitude != null)
            {
                Thread.Sleep(500);
                botClient.SendLocationAsync(message.Chat, (double) textEntry.FirstLatitude, (double) textEntry.FirstLongitude);
                Thread.Sleep(500);
                botClient.SendLocationAsync(message.Chat, (double) textEntry.SecondLatitude, (double) textEntry.SecondLongitude);
            }

            chatUsers.GetUsers.AddOrUpdate(user);
            chatUsers.SaveChanges();
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await Task.Run(() => { Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception)); });
        }
    }
}