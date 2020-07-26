using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core
{
    public class Like
    {
        public int LikerId { get; set; }
        public int LikedId { get; set; }
        public User Liker { get; set; }
        public User Liked { get; set; }
    }
}
