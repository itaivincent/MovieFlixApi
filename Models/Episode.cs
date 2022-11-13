using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieFlixApi.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public string Show_id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Viewed { get; set; }
    }
}
