using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterSampleStream.Models
{
    public class TweetStatistic
    {
        
        public int ID { get; set; }
        public string TweetID { get; set; }
        public DateTime DateTime { get; set; }
        public List<Emoji> Emojis { get; set; } = new List<Emoji>();
        public List<HashTag> HashTags { get; set; } = new List<HashTag>();
        public List<Url> Urls { get; set; } = new List<Url>();
        public bool HasUrl { get { return Urls.Count > 0; } }
        public bool HasHashTags { get { return HashTags.Count > 0; } }
        public bool HasImage { get; set; }
        public bool HasEmoji { get { return Emojis.Count > 0; } }

    }

    public class Emoji
    {
        public TweetStatistic TweetStatistic { get; set; }
        public int TweetStatisticID { get; set; }
        public string Unicode { get; set; }
        public string Variation { get; set; }
        public string Name { get; set; }        
        public int ID { get; set; }
        public int SpriteX { get; set; }
        public int SpriteY { get; set; }
        public string Image { get; set; }
    }

    public class HashTag
    {
        public TweetStatistic TweetStatistic { get; set; }
        public int TweetStatisticID { get; set; }
        public string TagText { get; set; }
        public int ID { get; set; }
    }

    public class Url    {
        public TweetStatistic TweetStatistic { get; set; }
        public int TweetStatisticID { get; set; }
        public string Domain { get; set; }
        public int ID { get; set; }

    }
}
