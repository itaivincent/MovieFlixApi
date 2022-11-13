using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieFlixApi.Models
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Release_Date { get; set; }
        public string Image { get; set; }
        public DateTime Date_Added { get; set; }
        public string Category { get; set; }

    }
}
