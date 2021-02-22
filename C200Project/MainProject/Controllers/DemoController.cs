using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using WebApplication1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using MainProject.Models;

namespace MainProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {

        private IWebHostEnvironment _env;
        public DemoController(IWebHostEnvironment environment)
        {
            _env = environment;
        }

        private string DoPhotoUpload(IFormFile photo)
        {
            string fext = Path.GetExtension(photo.FileName);
            string uname = Guid.NewGuid().ToString();
            string fname = uname + fext;
            string fullpath = Path.Combine(_env.WebRootPath, "candidates/" + fname);
            FileStream fs = new FileStream(fullpath, FileMode.Create);
            photo.CopyTo(fs);
            fs.Close();
            return fname;
        }

        // GET api/demo
        [HttpGet]

        public IEnumerable<UserDetails> Get()
        {
            List<UserDetails> dbList = DBUtl.GetList<UserDetails>("SELECT * FROM UserDetails");
            return dbList;
        }

        // GET api/demo/batman
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {

            List<UserDetails> dbList = DBUtl.GetList<UserDetails>($"SELECT * FROM UserDetails WHERE UserName='{name}'");
            if (dbList.Count > 0)
                return Ok(dbList[0]);
            else
                return NotFound();
        }

        [HttpPost("upload", Name = "upload")]
        public IActionResult UploadFile([FromForm]string username, 
            [FromForm] string email, 
            [FromForm] string password, 
            IFormFile photo)
        {
            if (username == null || email == null || password == null) 
            {
                return BadRequest("Parameters must not be null");
            }

            //string fname = "";
            string fname = DoPhotoUpload(photo);

            string sqlInsert = @"INSERT INTO UserDetails(UserName, Email, Password, Image) VALUES
                        ('{0}','{1}','{2}','{3}');";
            if (DBUtl.ExecSQL(sqlInsert, username, email, password, fname) == 1)
                return Ok();
            else
                return BadRequest(new { Message = DBUtl.DB_Message });
        }

        [HttpPost("habib", Name = "habib")]
        public IActionResult UploadHabib(IFormFile photo)
        {
           
            string fname = DoPhotoUpload(photo);

            string sqlInsert = @"INSERT INTO Habib(Image) VALUES
                        ('{0}');";
            if (DBUtl.ExecSQL(sqlInsert,fname) == 1)
                return Ok();
            else
                return BadRequest(new { Message = DBUtl.DB_Message });
        }

        //[HttpPost("upload", Name = "upload")]
        //public IActionResult UploadFile(IFormFile photo)
        //{
        //    string fname = DoPhotoUpload(photo);
        //    return Ok();
        //}

        // PUT api/demo/superman
        [HttpPut("{name}")]
        public IActionResult Put(string name, [FromBody] UserDetails rec)
        {
            if (rec == null || name == null)
            {
                return BadRequest();
            }

            string sql = @"UPDATE UserDetails 
                           SET Email ='{1}',
                               Password = '{2}',
                               Image = '{3}'
                         WHERE UserName'{0}'";
            string update = String.Format(sql, rec.UserName, rec.Email, rec.Password, rec.Image);
            if (DBUtl.ExecSQL(update) == 1)
                return Ok();
            else
                return BadRequest(new { Message = DBUtl.DB_Message });
        }

        // DELETE api/demo/wonderwoman
        [HttpDelete("{id}")]
        public IActionResult Delete(string name)
        {
            if (name == null)
            {
                return BadRequest();
            }

            string sql = @"DELETE UserDetails 
                         WHERE UserName={0}";
            string delete = String.Format(sql, name);
            if (DBUtl.ExecSQL(delete) == 1)
                return Ok();
            else
                return BadRequest(new { Message = DBUtl.DB_Message });

        }
    }
}

