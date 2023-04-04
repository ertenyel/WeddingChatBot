using System.ComponentModel.DataAnnotations;

namespace WeddingChatBot.DataModel
{
    public class Button
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public int? IdMessage { get; set; }
        public int? ButtonType { get; set; }
    }
}
