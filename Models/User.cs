using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieFlixApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DoB { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string confirm_password { get; set; }
    }
}
