using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MainProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;

namespace MainProject.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            List<UserDetails> lstCandidate =
               DBUtl.GetList<UserDetails>("SELECT * FROM UserDetails ORDER BY UserName");
            return View(lstCandidate);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Display(string name)
        {
            string sql = String.Format(@"SELECT * FROM UserDetails
                                       WHERE UserName = {0}", name);
            List<UserDetails> lstCandidate = DBUtl.GetList<UserDetails>(sql);
            if (lstCandidate.Count == 0)
            {
                TempData["Message"] = $"Candidate #{name} not found";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Index");
            }
            else
            {
                // Get the FIRST element of the List
                UserDetails cdd = lstCandidate[0];
                return View(cdd);
            }
        }

        public async Task<string> SnapShot(IFormFile upimage)
        {
            string filename = Guid.NewGuid().ToString() + ".jpg";
            string fullpath = Path.Combine(_env.WebRootPath, @"candidates\" + filename);
            using (FileStream fs = new FileStream(fullpath, FileMode.Create))
            {
                upimage.CopyTo(fs);
                fs.Close();
            }
            return filename;
        }


        // To Present An Emtpy Form
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // To Handle Post Back Input Data 
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Create(UserDetails user)
        {
            ModelState.Remove("Photo");
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View(user);
            }
            else
            {
                //user.Image = Path.GetFileName(photo.FileName);
                //string fname = "candidates/" + user.Image;
                //UploadFile(photo, fname);

                string sql = @"INSERT INTO UserDetails(UserName, Email, Password, Image)
              VALUES('{0}','{1}','{2}','{3}')";


                string insert = String.Format(sql, user.UserName, user.Email, user.Password, user.Image);

                int count = DBUtl.ExecSQL(insert);
                if (count == 1)

                {
                    TempData["Message"] = $"User {user.UserName} Successfully Added.";
                    TempData["MsgType"] = "success";
                    return RedirectToAction("login", "Account");
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                    return View(user);
                }

            }
        }
        #region Update
        [HttpGet]
        [Authorize]
        public IActionResult Update()
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            string sql = @"SELECT * FROM UserDetails
                         WHERE UserName='{0}'";

            string select = String.Format(sql, userid);
            List<UserDetails> update = DBUtl.GetList<UserDetails>(select);
            if (update.Count == 1)
            {
                UserDetails updateUser = update[0];
                return View(updateUser);
            }
            else
            {
                TempData["Message"] = "User does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Profile", "Main");
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult Update(UserDetails user)
        {
            ModelState.Remove("Photo");
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "danger";
                return View(user);
            }
            else
            {

                string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                //string picfilename = UploadFile(user.Photo);
                string sql = @"UPDATE UserDetails
                           SET Email   = '{1}',
                               Password = '{2}',
                               Image = '{3}'
                         WHERE UserName = '{0}'";

                if (DBUtl.ExecSQL(sql, userid, user.Email, user.Password, user.Image) == 1)
                {
                    TempData["Message"] = $"User {userid} Successfully Updated";
                    TempData["MsgType"] = "success";
                    return RedirectToAction("Profile","Main");
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                    return View();

                }
                //return RedirectToAction("Profile");
            }
        }
        #endregion
        private IWebHostEnvironment _env;
        public RegisterController(IWebHostEnvironment environment)
        {
            _env = environment;
        }
        private void UploadFile(IFormFile ufile, string fname)
        {
            string fullpath = Path.Combine(_env.WebRootPath, fname);
            using (var fileStream = new FileStream(fullpath, FileMode.Create))
            {
                ufile.CopyToAsync(fileStream);
            }
        }
    }


}

