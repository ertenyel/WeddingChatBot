using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace WeddingChatBot.DataModel
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public long TelegramCode { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Choice { get; set; }
        public string Companion { get; set; }
        public string Alcohol { get; set; }
        public string Food { get; set; }
        public int IdChatPosition { get; set; }
    }
}
