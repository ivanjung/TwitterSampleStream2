using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterSampleStream.Models
{

    public class EmojiMapping
    {
        public string name { get; set; }
        public string unified { get; set; }
        public string non_qualified { get; set; }
        public string image { get; set; }
        public int sheet_x { get; set; }
        public int sheet_y { get; set; }
        public string short_name { get; set; }
        public string category { get; set; }
        public int sort_order { get; set; }
        public bool has_img_apple { get; set; }
        public bool has_img_google { get; set; }
        public bool has_img_twitter { get; set; }
        public bool has_img_facebook { get; set; }
    }
}
