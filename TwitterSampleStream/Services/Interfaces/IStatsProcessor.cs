using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterSampleStream.Models;

namespace TwitterSampleStream.Services
{
    public interface IStatsProcessor
    {
        void GetTweets();
        double TweetsPerMinute();
        int TotalTweets();
        double TweetsWithUrlPercent();
        double TweetsWithPhotoPercent();
        List<Emoji> TopEmojis();
        List<string> TopHashTags();
        List<string> TopDomains();
    }
}
