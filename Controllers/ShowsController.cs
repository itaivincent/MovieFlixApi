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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MovieFlixApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        //dependency injection the database connection class

        public ShowsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //method to get all shows from the DB
       [HttpGet]
       [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
       public JsonResult Get()
        {
            string query = @"
                            select Id,Name,Description,Release_Date,Image,Date_Added,Category from
                            dbo.Shows
                           ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MovieFlixConnection");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }


        //method to insert a show the DB
        [HttpPost]
        public JsonResult Post(Show show)
        {
            string query = @"
                            insert into dbo.Shows 
                            values(@Name,@Description,@Release_Date,@Image,@Date_Added,@Category,@User_Id,@Imdb_id)
                           ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MovieFlixConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Name", show.Name);
                    myCommand.Parameters.AddWithValue("@Description", show.Description);
                    myCommand.Parameters.AddWithValue("@Release_Date", show.Release_Date);
                    myCommand.Parameters.AddWithValue("@Image", show.Image);
                    myCommand.Parameters.AddWithValue("@Date_Added", show.Date_Added);
                    myCommand.Parameters.AddWithValue("@Category", show.Category);
                    myCommand.Parameters.AddWithValue("@User_id", show.User_id);
                    myCommand.Parameters.AddWithValue("@Imdb_id",show.Imdb_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("New Show Added Successfully");
        }


        //get all shows by a specific user
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult GetShow(int id)
        {
            string query = @"select * from dbo.Shows
                            where User_id = @User_id";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MovieFlixConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@User_id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }


        //this is a method to update a record in the database, the method type is patch
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult updateShow(int id)
        {
            //string query = @"select * from dbo.Shows
            //                where User_id = @User_id"; 

            string query = @"update dbo.Shows set IsWatched = 'True' where Id = @Id";

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

            return new JsonResult(table);
        }

        //method to delete a show from the DB
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                           delete from dbo.Shows
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

            return new JsonResult("Show removed successfully");
        }
    }
}
