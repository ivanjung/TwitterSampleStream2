using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models;
using TwitterSampleStream.Models;

namespace TwitterSampleStream.Services
{
    public interface IEmojiProcessing
    {

        List<EmojiMapping> ExtractEmojiUnicode(ITweet tweet);
    }
}
