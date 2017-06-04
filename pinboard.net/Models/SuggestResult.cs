using System;
using System.Collections.Generic;
using System.Text;

namespace pinboard.net.Models
{
    public class SuggestResult
    {
        public List<string> Popular { get; set; }
        public List<string> Recommended { get; set; }
    }
}
