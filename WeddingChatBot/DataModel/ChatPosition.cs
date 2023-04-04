using System.ComponentModel.DataAnnotations;

namespace WeddingChatBot.DataModel
{
    public class ChatPosition
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int? IdMessageText { get; set; }
    }
}
