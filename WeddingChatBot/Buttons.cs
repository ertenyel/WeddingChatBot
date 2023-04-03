using System.Collections.Generic;
using System.Reflection.Emit;
using Telegram.Bot.Types.ReplyMarkups;

namespace WeddingChatBot
{
    public static class Buttons
    {
        public static ReplyKeyboardMarkup YesNoOrMayBe()
        {
            return new ReplyKeyboardMarkup(new List<KeyboardButton>()
                                           {
                                               new KeyboardButton(Answer.GetAnswerText(AnswerKey.Yes)),
                                               new KeyboardButton(Answer.GetAnswerText(AnswerKey.No)),
                                               new KeyboardButton(Answer.GetAnswerText(AnswerKey.MayBe))
                                           });
        }
        public static ReplyKeyboardMarkup YesOrNo()
        {
            return new ReplyKeyboardMarkup(new List<KeyboardButton>()
                                           {
                                               new KeyboardButton(Answer.GetAnswerText(AnswerKey.Yes)),
                                               new KeyboardButton(Answer.GetAnswerText(AnswerKey.No)),
                                           });
        }
        public static AnswerKey GetUserChoice(string userAnswer)
        {
           return Answer.GetAnswerKey(userAnswer);
        }
    }
}
