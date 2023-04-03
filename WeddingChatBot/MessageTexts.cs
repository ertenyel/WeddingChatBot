using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace WeddingChatBot
{
    public class MessageTexts
    {
        public MessageTexts(string idChatPosition, Message message)
        {
            if (idChatPosition == "start")
            {
                Start(message.From.FirstName);
            }
            else if (idChatPosition == "responsetoinvite")
            {
                ResponseToInvite(message.Text);
            }
            else if (idChatPosition == "otherpeople")
            {
                OtherPeople(message.Text);
            }
            else if (idChatPosition == "waitreason")
            {
                WaitReason();
            }
            else if (idChatPosition == "selectalcohol")
            {
                SelectAlcohol();
            }
            else if (idChatPosition == "writeotherpeople")
            {
                WriteOtherPeople();
            }
            else if (idChatPosition == "end")
            {
                End();
            }
        }
        public string Text { get; set; }
        public IReplyMarkup Keyboard { get; set; }

        private void Start(string name)
        {
            Text = $"Привет, {name}!\n\nСпешим сообщить радостную новость - мы создаем семью!\nМы это те, что на первой фотке! Гляньте и другие фотографии тоже\nКороче! Приглашаем тебя разделить с нами радость этого долгожданного события!\n\nТы будешь?";
            Keyboard = Buttons.YesNoOrMayBe();
        }
        private void ResponseToInvite(string message)
        {
            AnswerKey chatAnswer = Answer.GetAnswerKey(message);
            if (chatAnswer == AnswerKey.Yes || chatAnswer == AnswerKey.MayBe)
            {
                Text = chatAnswer == AnswerKey.Yes ? "Просто супер! С тобой будет вторая половинка?" : "Ты действительно думал, что ответ будет другой если ты нажмешь \"Увидим...\"? Ты думай дальше и напиши будет ли вторая половинка?";
                Keyboard = Buttons.YesOrNo();
            }
            else if (chatAnswer == AnswerKey.No)
            {
                Text = "Почему это? Можешь ничего не отвечать";
                Keyboard = new ReplyKeyboardRemove();
            }
        }
        private void OtherPeople(string message)
        {
            AnswerKey chatAnswer = Answer.GetAnswerKey(message);
            if (chatAnswer == AnswerKey.Yes)
            {
                Text = "Хорошо. Расскажи предпочтения по алкоголю?\n Пиши в любом формате (в литрах/бутылках/цистернах) и с любым названием (\"помнишь тогда пили\", \"12-летний коньяк\" и т.д.)";
            }
            else if (chatAnswer == AnswerKey.No)
            {
                Text = "Напиши имя второй половинки, которая будет с вами на празднике!";
            }
            Keyboard = new ReplyKeyboardRemove();
        }
        private void WaitReason()
        {
            Text = "Записал, но надеюсь, что ты передумаешь.\n Нажимай сюда и бот запустится заново /start";
            Keyboard = new ReplyKeyboardRemove();
        }
        private void SelectAlcohol()
        {
            Text = "Ответ зафиксировал!\nЖдем!";
            Keyboard = new ReplyKeyboardRemove();
        }
        private void WriteOtherPeople()
        {
            Text = "Отлично!\nчто хотели бы выпивать?";
            Keyboard = new ReplyKeyboardRemove();
        }
        private void End()
        {
            Text = "Ваш ответ я записал. К сожалению, я не настолько крут, чтобы говорить еще о чём-то.\n Если хотите пройти ответы заново, то нажмите /start";
            Keyboard = new ReplyKeyboardRemove();
        }
    }
}
