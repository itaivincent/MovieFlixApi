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
        public string Email { get; set; }
        public string Password { get; set; }
        public string Confirm_password { get; set; }
        public string Token { get; set; }
    }
}
