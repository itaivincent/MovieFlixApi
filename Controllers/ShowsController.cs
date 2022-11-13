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
                            values(@Name,@Description,@Release_Date,@Image,@Date_Added,@Category)
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
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("New Show Added Successfully");
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
