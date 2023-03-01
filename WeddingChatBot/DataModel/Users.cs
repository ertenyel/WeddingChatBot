using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace WeddingChatBot.DataModel
{
    public class Users
    {
        [Key]
        public int UsersId { get; set; }
        public long TelegramCode { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Choice { get; set; }
        public string PeopleGoTogether { get; set;}
        public string Alcohol { get; set; }
        public string IdChatPosition { get; set; }
    }
}
