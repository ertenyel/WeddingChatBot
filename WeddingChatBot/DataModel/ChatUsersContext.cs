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

        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}