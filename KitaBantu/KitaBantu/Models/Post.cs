using System;
using System.Collections.Generic;
using System.Text;

namespace KitaBantu.Models
{
    public class Post
    {
        public string postId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string userId { get; set; }
        public string imageUrl { get; set; }
        public DateTime datetimePosted { get; set; }
        public bool isCompleted { get; set; }
    }
}
