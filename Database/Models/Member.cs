using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class Member
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Sign { get; set; }
        public string Avatar { get; set; }
    }
}
