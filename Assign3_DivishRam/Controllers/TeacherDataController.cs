using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;

namespace BlogProject.Controllers
{
    public class TeacherDataController : ApiController
    {
        //Connection to MySQL Database
        private SchoolDbContext School = new SchoolDbContext();


        ///<summary>
        ///</summary>
        ///

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<string> ListTeachers(string SearchKey = null)    
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key) or lower(salary) like lower(@key) or lower(hiredate) like lower(@key)";
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teacher
            List<String> Teachers = new List<Teachers> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index'
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime Hire = Convert.ToDateTime(ResultSet["hiredate"].ToString());
                Decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                Teacher NewTeacher = new Teacher();

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.TeacherHireDate = Hire;
                NewTeacher.TeacherSalary = Salary;

                Teachers.Add(NewTeacher);
            }

            //Close Connection to MySql DB
            Conn.Close();
            return NewTeacher;
        }

        ///summary
        ///Find teacher with specified id
        ///

        [HttpGet]
        public string FindTeacher(int id)
        {
            string query = "SELECT * FROM teachers WHERE teacherid =" + id;
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand Cmd = Conn.CreateCommand();
            Cmd.CommandText = query;
            MySqlDataReader ResultSet = Cmd.ExecuteReader();

            string TeacherName = "";
            while (ResultSet.Read())
            {
                TeacherName = ResultSet["teacherfname"] + " " + ResultSet["teacherlname"];
            }

            Conn.Close();
            return TeacherName;
        }

        





    }
}
