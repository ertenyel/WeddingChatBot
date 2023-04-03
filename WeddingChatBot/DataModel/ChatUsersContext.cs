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
        }

        public virtual DbSet<Users> GetUsers { get; set; }
    }
}