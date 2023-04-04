using System;
using System.Data.Entity;
using System.Linq;

namespace WeddingChatBot.DataModel
{
    public class ChatUsersContext : DbContext
    {
        public ChatUsersContext()
            : base("name=ChatUsers")
        {
            Database.CreateIfNotExists();
        }

        public virtual DbSet<User> GetUsers { get; set; }
        public virtual DbSet<ChatPosition> ChatPositions { get; set; }
        public virtual DbSet<TextInMessage> TextsInMessage { get; set; }
        public virtual DbSet<Button> Buttons { get; set; }
    }
}