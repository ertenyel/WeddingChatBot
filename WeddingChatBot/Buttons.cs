using System.Collections.Generic;
using System.Reflection.Emit;
using Telegram.Bot.Types.ReplyMarkups;

namespace WeddingChatBot
{
    public static class Buttons
    {
        public static ReplyKeyboardMarkup MainButtons 
        { 
            get => new ReplyKeyboardMarkup(new List<KeyboardButton>()
        {
            new KeyboardButton(MainMenuButtons.InfoButton),
            new KeyboardButton(MainMenuButtons.UserInfoButton)
        });
        }
        public static ReplyKeyboardMarkup InfoButtons
        {
            get => new ReplyKeyboardMarkup(new List<KeyboardButton>()
        {
            new KeyboardButton(InfoEventButtons.InfoAboutNewlyweds),
            new KeyboardButton(InfoEventButtons.EventPlan),
            new KeyboardButton(InfoEventButtons.Locations),
            new KeyboardButton(InfoEventButtons.ColorClothing)
        });
        }
        public static ReplyKeyboardMarkup AboutUserButtons
        {
            get => new ReplyKeyboardMarkup(new List<KeyboardButton>()
        {
            new KeyboardButton(UserInfoButtons.Choice),
            new KeyboardButton(UserInfoButtons.Companion),
            new KeyboardButton(UserInfoButtons.Alcohol),
            new KeyboardButton(UserInfoButtons.Food)
        });
        }
        public static ReplyKeyboardMarkup OtherButton
        {
            get => new ReplyKeyboardMarkup(new List<KeyboardButton>()
        {
            new KeyboardButton(OtherButtons.Exit),
            new KeyboardButton(OtherButtons.ExitToMenu)
        });
        }
    }
}
