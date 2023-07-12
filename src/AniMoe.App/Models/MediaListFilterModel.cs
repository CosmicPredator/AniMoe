using AniMoe.App.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniMoe.App.Models
{
    public class MediaListFilterModel
    {
        public Country Country { get; set; }
        public int? Year { get; set; }
        public Format Format { get; set; }
        public Status Status { get; set; }
        public List<string> Genres { get; set; }
    }

    public class Genre
    {
        public string Name { get; set; }
        public bool IsChekced { get; set; }
    }
}
