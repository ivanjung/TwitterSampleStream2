using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models;
using TwitterSampleStream.Models;
using TwitterSampleStream.Services;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TwitterSampleStream.Services

{
    public class EmojiProcessing: IEmojiProcessing
    {
        private readonly IEmojiData emojiService;
        private readonly ILogger _logger;
        public EmojiProcessing(IEmojiData emojiService, ILoggerFactory loggerFactory)
        {
            this.emojiService = emojiService;
            this._logger = loggerFactory.CreateLogger<EmojiProcessing>();
        }
        public List<EmojiMapping> ExtractEmojiUnicode(ITweet tweet)
        {
            List<EmojiMapping> retList = new List<EmojiMapping>();
            try
            {
                var fullText = tweet.FullText;
                List<EmojiMapping> emojiData = emojiService.GetEmojiMappings();
                foreach (var mapping in emojiData)
                {
                    string searchString = string.Join(string.Empty, mapping.unified.Split('-')
                                                                    .Select(hex => char.ConvertFromUtf32(Convert.ToInt32(hex, 16))));
                    if (fullText.Contains(searchString))
                    {
                        retList.Add(mapping);
                    }
                }

            }
            catch(Exception ex)
            {
                _logger.LogError(String.Format("EmojiProcessing.ExtractEmojiUnicode - {0}", ex.Message), ex);
            }
            
            

            return retList;
        }
    }
}
