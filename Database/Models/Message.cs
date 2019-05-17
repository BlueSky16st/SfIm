using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int ToId { get; set; }
        public byte[] Timestamp { get; set; }
        public DateTime DateTimeStr { get; set; }
        public int MessageType { get; set; }
    }
}
