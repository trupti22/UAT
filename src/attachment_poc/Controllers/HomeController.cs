using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Runtime;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Net.Http;

namespace attachment_poc.Controllers
{
    public class HomeController : Controller
    {
        /***********/
        public IHostingEnvironment hostingEnv;
        public object fileName;

        public HomeController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }

        public IActionResult UploadFiles()
        {
            /*String connectionString = "Data Source = 203.193.138.52,6148; Initial Catalog = POC_UAT; Persist Security Info = True; User ID = ttsh_crio; Password = Newuser@123";
            String sql = "SELECT * FROM fileUpload";
            SqlCommand cmd = new SqlCommand(sql);
            var model = new List<fileU>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                cmd.Connection = conn;
                SqlDataReader rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    var fileInfo = new fileU();
                    fileInfo.Name = rdr["Name"].ToString();
                    byte[] bytes = (byte[])rdr["Data"];

                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    fileInfo.Data = "data:image/png;base64," +base64String;
                    
                    model.Add(fileInfo);

                    //Response.ContentType = rdr["ContentType"].ToString();          
                    //Response.Headers.Add("content-disposition", "attachment;filename=" + fileInfo.Name);
                    //Response.Body.WriteAsync(bytes, 0, bytes.Length);
                }
            }            
            return View(model);*/
             return View();
        }

        public IActionResult download()
        {
            String connectionString = "Data Source = 203.193.138.52,6148; Initial Catalog = POC_UAT; Persist Security Info = True; User ID = ttsh_crio; Password = Newuser@123";
            String sql = "SELECT * FROM fileUpload where id = 1";
            SqlCommand cmd = new SqlCommand(sql);
            var model = new List<fileU>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                cmd.Connection = conn;
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var fileInfo = new fileU();
                    fileInfo.Name = rdr["Name"].ToString();
                    byte[] bytes = (byte[])rdr["Data"];

                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    fileInfo.Data = "data:image/png;base64," + base64String;

                    model.Add(fileInfo);

                    Response.ContentType = rdr["ContentType"].ToString();          
                    Response.Headers.Add("content-disposition", "attachment;filename=" + fileInfo.Name);
                    Response.Body.WriteAsync(bytes, 0, bytes.Length);
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult UploadFiles(IList<IFormFile> files)
        {
            foreach (var file in files)
            {
                string filename = Path.GetFileName(file.FileName);
                string contentType = file.ContentType;

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var fileContent = reader.ReadToEnd();
                   
                    BinaryReader br = new BinaryReader(file.OpenReadStream());
                    byte[] bytes = br.ReadBytes((Int32)fileContent.Length);

                    using (SqlConnection con = new SqlConnection("Data Source =192.168.1.152,32768; Initial Catalog =RGenUAT; Persist Security Info =True;User ID=uatadmin;Password=Newuser@123"))
                    {
                        string query = "insert into fileUpload values (@Name, @ContentType, @Data)";
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@Name", filename);
                            cmd.Parameters.AddWithValue("@ContentType", contentType);
                            cmd.Parameters.AddWithValue("@Data", bytes);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }               
            }        
            return View();        
        }
        /**********/
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
