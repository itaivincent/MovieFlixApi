using MovieFlixApi.IRepository;
using MovieFlixApi.Models;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace MovieFlixApi.Repository
{
    public class UserRepository : IUserRepository
    {
        string _connectionString = "";
        User  _oUser = new User();
       // List<User> _oUsers= new List<User>();

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MovieFlixConnection");
        }
        public async Task<User> Get(int objId)
        {
            _oUser = new User();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var Users = await con.QueryAsync<User>(string.Format(@"SELECT * FROM Users WHERE Id={0}",objId));
                if(Users != null && Users.Count() > 0)
                {
                    _oUser = Users.SingleOrDefault();
                }
            }

            return _oUser;

        }

        public async Task<User> GetByEmailPassword(User user)
        {
            _oUser = new User();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string sql = string.Format(@"SELECT * FROM [Users] WHERE Email='" + user.Email + "'AND Password='" + user.Password + "'");
                var users = await con.QueryAsync<User>(sql);
                if (users != null && users.Count() > 0)
                {
                    _oUser = users.SingleOrDefault();
                }
            }

            return _oUser;
        }
    }
}
