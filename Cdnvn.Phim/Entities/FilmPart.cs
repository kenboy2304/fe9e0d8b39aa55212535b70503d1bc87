using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdnvn.Phim.Entities
{
    public class FilmPart:General
    {
        public string YoutubeId { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public bool isError { get; set; }

        public int FilmId { get; set; }
        public virtual Film Film { get; set; }
        
    }
}
