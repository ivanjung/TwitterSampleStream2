using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TwitterSampleStream.Models;

namespace TwitterSampleStream.Services
{
    public class EmojiData : IEmojiData
    {
        public List<EmojiMapping> GetEmojiMappings()
        {
            string path = Directory.GetCurrentDirectory() + @"\emoji.json";
            List<EmojiMapping> emojis = null;
            try
            {
                using (StreamReader file = File.OpenText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    emojis = (List<EmojiMapping>)serializer.Deserialize(file, typeof(List<EmojiMapping>));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            return emojis;
        }
    }
}
