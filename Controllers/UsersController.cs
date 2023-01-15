using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MovieFlixApi.Models;
using System.Net;
using MovieFlixApi.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;


namespace MovieFlixApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IUserRepository _oUserRepository;

        //dependency injection the database connection class
        public UsersController(IConfiguration configuration, IUserRepository oUserRepository)
        {
            _configuration = configuration;
            _oUserRepository = oUserRepository;

        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> login([FromBody] User obj)
        {

            User model = new User()
            {
                Email = obj.Email,
                Password = obj.Password
            };

            var user = await AuthenticationUser(model);


            if (user.Id == 0) return StatusCode((int)HttpStatusCode.NotFound, "User not found");
            user.Token = GenerateToken(model);
            return Ok(user);

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {

            var user = await GettingUser(id);
            if (user.Id == 0) return StatusCode((int)HttpStatusCode.NotFound, "User not found");
            return Ok(user);
        }

        private string GenerateToken(User model)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"], null,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User> AuthenticationUser(User user)
        {
            return await _oUserRepository.GetByEmailPassword(user);

        }


        private async Task<User> GettingUser(int id)
        {
            return await _oUserRepository.Get(id);

        }

        //method to insert a user the DB
        [HttpPost]
        [Route("registration")]
        public JsonResult registration(User user)
        {
            string query = @"
                            insert into dbo.Users 
                            values(@Name,@Surname,@DoB,@Email,@Password,@Confirm_password,@Token)
                           ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MovieFlixConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Name", user.Name);
                    myCommand.Parameters.AddWithValue("@Surname", user.Surname);
                    myCommand.Parameters.AddWithValue("@DoB", user.DoB);
                    myCommand.Parameters.AddWithValue("@email", user.Email);
                    myCommand.Parameters.AddWithValue("@Password", user.Password);
                    myCommand.Parameters.AddWithValue("@Confirm_password", user.Confirm_password);
                    myCommand.Parameters.AddWithValue("@Token", user.Token);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("New User Added Successfully");
        }



        //method to delete a user from the DB
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                           delete from dbo.Users
                            where Id = @Id
                           ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MovieFlixConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("User removed successfully");
        }


    }
}
