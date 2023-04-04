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
        public virtual DbSet<ChatPosition> GetChatPositions { get; set; }
        public virtual DbSet<TextInMessage> GetTextsInMessage { get; set; }
        public virtual DbSet<Button> GetButtons { get; set; }
    }
}