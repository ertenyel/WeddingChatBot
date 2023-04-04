using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeddingChatBot.DataModel
{
    public class TextInMessage
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public string StickerId { get; set; }
        public double? FirstLatitude { get; set; }
        public double? FirstLongitude { get; set; }
        public double? SecondLatitude { get; set; }
        public double? SecondLongitude { get; set; }
        public int? IdButtons { get; set; }
    }
}
