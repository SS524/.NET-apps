using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalMicroservice.Model
{
    public class Tweet
    {
        public int Id { get; set; }   // Tweet ID
       
        public string Post { get; set; } //TWEET BODY
        
        public string PostedBy { get; set; }

       
        public int ReplyID { get; set; }

        
        public int LikeCount { get; set; }

        public string LikedBy { get; set; }
    }
}
