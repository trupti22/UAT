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
            return View();
        }


       
        //  foreach(var file in Request.Files)
        //
        [HttpPost]
        public IActionResult UploadFiles(IList<IFormFile> files)
        {          
            foreach (var file in files)
            {
                var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fileNameTrim = "";
                /*Solution for IE issue*/
                int position = filename.LastIndexOf('\\');

                if (position != -1)
                    fileNameTrim = filename.Substring(position + 1);
                else
                    fileNameTrim = filename;
                /**/
                filename = hostingEnv.WebRootPath + $@"\Attchment\{fileNameTrim}";
                
                using (FileStream fs=System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
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
