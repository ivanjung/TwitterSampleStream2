using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models;
using TwitterSampleStream.DAL;
using TwitterSampleStream.Models;
namespace TwitterSampleStream.Services
{
    public class TweetProcessing : ITweetProcessing
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger _logger;
        public TweetProcessing(IServiceScopeFactory scopeFactory, ILoggerFactory loggerFactory)
        {
            this.scopeFactory = scopeFactory;
            _logger = loggerFactory.CreateLogger<TweetProcessing>();
        }
        async Task ITweetProcessing.ProcessTweet(ITweet tweet)
        {
            try
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    //create separate classes for each of these "stats" 
                    var dbContext = scope.ServiceProvider.GetService<TweetsContext>();
                    var tweetStats = new TweetStatistic { DateTime = tweet.CreatedAt.DateTime, TweetID = tweet.IdStr };

                    tweetStats.HashTags.AddRange(GetHashTags(tweet, tweetStats));

                    tweetStats.Urls.AddRange(GetUrls(tweet, tweetStats));

                    //emojiparser service 
                    var emojiProccesor = scope.ServiceProvider.GetService<IEmojiProcessing>();
                    var listOfEmojis = emojiProccesor.ExtractEmojiUnicode(tweet);
                    tweetStats.Emojis.AddRange(GetEmojis(tweet, tweetStats, listOfEmojis));


                    if (tweet.Media != null && tweet.Media.Count > 0)
                    {
                        tweetStats.HasImage = tweet.Media.Any(media => (media.MediaType == "photo" || media.MediaType == "animated_gif"));
                    }

                    //make own service for saving to DB?
                    dbContext.Add(tweetStats);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(String.Format("TweetProcessing.ProcessTweet - {0} ",ex.Message), ex);
            }
         
            
        }

        private List<Emoji>GetEmojis(ITweet tweet, TweetStatistic tweetStats, List<EmojiMapping> listOfEmojis)
        {
            List<Emoji> emojis = new List<Emoji>();
            if (listOfEmojis != null && listOfEmojis.Count > 0)
            {
                foreach (var emojiCode in listOfEmojis)
                {
                    emojis.Add(new Emoji
                    {
                        Unicode = emojiCode.unified,
                        Variation = emojiCode.non_qualified,
                        SpriteX = emojiCode.sheet_x,
                        SpriteY = emojiCode.sheet_y,
                        Name = emojiCode.name,
                        TweetStatistic = tweetStats
                    });
                }
            }
            
            return emojis;
        }

        private List<HashTag> GetHashTags(ITweet tweet, TweetStatistic tweetStats)
        {
            var tags = new List<HashTag>();
            if (tweet.Hashtags != null && tweet.Hashtags.Count > 0)
            {
                foreach (var ht in tweet.Hashtags)
                {
                    tags.Add(new HashTag { TweetStatistic = tweetStats, TagText = ht.Text });
                }
            }           
            return tags;
        }
        private List<Url> GetUrls(ITweet tweet, TweetStatistic tweetStats)
        {
            var urls = new List<Url>();
            if (tweet.Urls != null && tweet.Urls.Count > 0)
            {
                foreach (var url in tweet.Urls)
                {
                    var uri = new Uri(url.ExpandedURL);
                    var host = uri.Host;                    
                    urls.Add(new Url { Domain = host, TweetStatistic = tweetStats });
                }
            }            
            return urls;
        }
    }
}
