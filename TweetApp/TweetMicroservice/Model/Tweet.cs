using System.ComponentModel.DataAnnotations;

namespace TweetMicroservice.Model
{
    public class Tweet
    {
        [Key]
        public int Id { get; set; }   // Tweet ID     
        public string Post { get; set; } //TWEET BODY       
        public string PostedBy { get; set; }     
        public int ReplyID { get; set; }
        public int LikeCount { get; set; }

        public string LikedBy { get; set; }

    }
}
