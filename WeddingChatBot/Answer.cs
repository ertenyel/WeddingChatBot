namespace WeddingChatBot
{
    public static class Answer
    {
        private const string Yes = "Да!";
        private const string No = "Нет!";
        private const string MayBe = "Увидим...";
        public static string GetAnswerText(AnswerKey key)
        {
            if (key == AnswerKey.Yes)
            {
                return Yes;
            }
            else if (key == AnswerKey.No)
            {
                return No;
            }
            else if (key == AnswerKey.MayBe)
            {
                return MayBe;
            }
            else
            {
                return string.Empty;
            }
        }
        public static AnswerKey GetAnswerKey(string text)
        {
            if (Yes == text)
            {
                return AnswerKey.Yes;
            }
            else if (No == text)
            {
                return AnswerKey.No;
            }
            else if (MayBe == text)
            {
                return AnswerKey.MayBe;
            }
            else
            {
                return AnswerKey.AnswerIsEmpty;
            }
        }
    }
}
