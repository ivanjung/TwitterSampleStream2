using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterSampleStream.Models.ViewModels
{
    public class StatsViewModel
    {
        public int TotalTweets { get; set; }
        public double TweetsPerMinute { get; set; }
        public double TweetsWithUrlPercent { get; set; }
        public double TweetsWithPhotoPercent { get; set; }
        public List<Emoji> TopEmojis { get; set; } = new List<Emoji>();
        public List<string> TopHashTags { get; set; } = new List<string>();
        public List<string> TopDomains { get; set; } = new List<string>();

    }
}
