using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using WeddingChatBot.DataModel;

namespace WeddingChatBot
{
    public static class Keyboard
    {
        public static IReplyMarkup Get(ChatUsersContext db, User user)
        {
            List<List<KeyboardButton>> keyboardButtons = new List<List<KeyboardButton>>();
            List<List<KeyboardButton>> positionButtons = GetPositionButtons(db, user);
            List<KeyboardButton> exitButtons = GetExitButtons(db, user);

            foreach (List<KeyboardButton> keyboardButton in positionButtons)
                keyboardButtons.Add(keyboardButton);
            if (exitButtons != null)
                keyboardButtons.Add(exitButtons);

            if (keyboardButtons.Count > 0)
                return new ReplyKeyboardMarkup(keyboardButtons);
            else
                return new ReplyKeyboardRemove();
        }
        private static List<List<KeyboardButton>> GetPositionButtons(ChatUsersContext db, User user)
        {
            int? idMessage = db.GetChatPositions.Where(x => x.Id == user.IdChatPosition).Select(x => x.IdMessageText).First();
            List<List<KeyboardButton>> keyboardButtons = new List<List<KeyboardButton>>();

            if (db.GetTextsInMessage.Where(x => x.Id == idMessage && x.IdButtons != null).Count() > 0)
            {
                int? idButtons = db.GetTextsInMessage.Where(x => x.Id == idMessage && x.IdButtons != null).Select(x => x.IdButtons).First();
                List<string> buttons = db.GetButtons.Where(x => x.IdMessage == idButtons).Select(x => x.Text).ToList();
                List<KeyboardButton> tempKeyboards = new List<KeyboardButton>();

                foreach (string button in buttons)
                {
                    if (user.Alcohol == null)
                        user.Alcohol = string.Empty;
                    if (user.Food == null)
                        user.Food = string.Empty;

                    if (!user.Alcohol.Contains(button) && !user.Food.Contains(button))
                        keyboardButtons.Add(new List<KeyboardButton>() { new KeyboardButton(button) });
                }
            }

            return keyboardButtons;
        }
        private static List<KeyboardButton> GetExitButtons(ChatUsersContext db, User user)
        {
            int? parentChatPosition = db.GetChatPositions.Where(x => x.Id == user.IdChatPosition).Select(x => x.ParentId).First();
            if (parentChatPosition > 1)
            {
                List<string> returnsButtons = db.GetButtons.Where(x => x.ButtonType == 4 || x.ButtonType == 5).Select(x => x.Text).ToList();
                List<KeyboardButton> tempKeyboards = new List<KeyboardButton>();

                foreach (string button in returnsButtons)
                    tempKeyboards.Add(new KeyboardButton(button));

                return tempKeyboards;
            }
            else if (parentChatPosition == 1)
            {
                return new List<KeyboardButton>()
                       {
                           new KeyboardButton(db.GetButtons.Where(x => x.ButtonType == 5).Select(x => x.Text).First())
                       };
            }

            return null;
        }
    }
}
