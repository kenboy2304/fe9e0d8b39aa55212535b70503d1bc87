using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdnvn.Phim.Entities
{
    public class General
    {
        public int Id { get; set; }
        public string SEOName { get; set; }

        [DefaultValue(true)]
        public bool Published { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Order { get; set; }
    }
}
