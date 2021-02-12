using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using TwitterSampleStream.DAL;
using TwitterSampleStream.Models;

namespace TwitterSampleStream.Services
{
    public class StatsProcessor : IStatsProcessor
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger _logger;
        public StatsProcessor(IServiceScopeFactory scopeFactory, ILoggerFactory loggerFactory)
        {
            this.scopeFactory = scopeFactory;
            _logger = loggerFactory.CreateLogger<StatsProcessor>();
            this.GetTweets();
        }
        private List<TweetStatistic> tweets { get; set; } = new List<TweetStatistic>();
        private List<Emoji> Emojis { get; set; } = new List<Emoji>();
        private List<Url> Urls { get; set; } = new List<Url>();
        private List<HashTag> HashTags { get; set; } = new List<HashTag>();

        public void GetTweets()
        {
            DateTime now = DateTime.Now;
            using (var scope = scopeFactory.CreateScope())
            {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    var dbContext = scope.ServiceProvider.GetService<TweetsContext>();
                    tweets = dbContext.Set<TweetStatistic>().ToList();
                    Emojis = dbContext.Set<Emoji>().ToList();
                    Urls = dbContext.Set<Url>().ToList();
                    HashTags = dbContext.Set<HashTag>().ToList();
                }
            }
        }

        public List<string> TopDomains()
        {
            var topUrls = Urls.GroupBy(u => u.Domain)
                        .OrderByDescending(u => u.Count())
                        .Take(10)
                        .Select(u => u.FirstOrDefault().Domain).ToList();

            
            return topUrls;
        }

        public List<Emoji> TopEmojis()
        {            
            var topEmojis = Emojis
                        .GroupBy(e => e.Name)
                        .OrderByDescending(e => e.Count())
                        .Take(10)
                        .Select(e => e.FirstOrDefault()).ToList();

            return topEmojis;
            
        }

        public List<string> TopHashTags()
        {
            var topHashTags = HashTags
                        .GroupBy(h => h.TagText)
                        .OrderByDescending(h => h.Count())
                        .Take(10)
                        .Select(h => h.FirstOrDefault().TagText).ToList();

            
            return topHashTags;
        }

        public int TotalTweets()
        {
            return tweets.Count;            
        }

        public double TweetsPerMinute()
        {
            double tweetsPerMinute = 0.0;
            double timeSpan = (tweets.Max(t=> t.DateTime) - tweets.Min(t => t.DateTime)).TotalMinutes;
            double totalTweets = tweets.Count;
            tweetsPerMinute = totalTweets / timeSpan;

            return tweetsPerMinute;
        }

        public double TweetsWithPhotoPercent()
        {
            double tweetsWithPhoto = tweets.Where(t => t.HasImage).Count();
            double totalTweets = tweets.Count();
            double percentage = tweetsWithPhoto / totalTweets;

            return percentage;
        }

        public double TweetsWithUrlPercent()
        {
            double tweetsWithUrl = tweets.Where(t => t.HasUrl).Count();
            double totalTweets = tweets.Count();
            double percentage = tweetsWithUrl / totalTweets;

            return percentage;        
        }
    }
}
