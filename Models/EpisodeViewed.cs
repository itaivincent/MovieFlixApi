using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieFlixApi.Models
{
    public class EpisodeViewed
    {
        public int Id { get; set; }
        public string show_id { get; set; }
        public string episode_id { get; set; }
        public string user_id { get; set; }
        public string count { get; set; }
        public DateTime date_watched { get; set; }
    }
}
