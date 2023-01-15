using MovieFlixApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieFlixApi.IRepository
{
    public interface IUserRepository
    {
        Task<User> Get(int objId);
        Task<User> GetByEmailPassword(User user);
    }
}
