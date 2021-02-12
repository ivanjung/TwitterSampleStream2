using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterSampleStream.Models;
namespace TwitterSampleStream.Services
{
    public interface IEmojiData
    {
        List<EmojiMapping> GetEmojiMappings();
    }
}
