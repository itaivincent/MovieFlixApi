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

namespace MovieFlixApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        //dependency injection the database connection class
        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        //method to insert a user the DB
        [HttpPost]
        public JsonResult Post(User user)
        {
            string query = @"
                            insert into dbo.Users 
                            values(@Name,@Surname,@DoB,@email,@password,@confirm_password)
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
                    myCommand.Parameters.AddWithValue("@email", user.email);
                    myCommand.Parameters.AddWithValue("@password", user.password);
                    myCommand.Parameters.AddWithValue("@confirm_password", user.confirm_password);
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
