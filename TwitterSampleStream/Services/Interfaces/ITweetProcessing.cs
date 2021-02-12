using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models;
namespace TwitterSampleStream.Services
{
    public interface ITweetProcessing
    {
        Task ProcessTweet(ITweet tweet);
    }
}
