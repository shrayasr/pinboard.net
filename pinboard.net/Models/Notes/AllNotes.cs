using System.Collections.Generic;

namespace pinboard.net.Models
{
    public class AllNotes
    {
        public int Count { get; set; }
        public List<Note> Notes { get; set; }
    }
}
