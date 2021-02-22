using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MainProject.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.Storage;
using Chart = MainProject.Models.Chart;
using System.Linq;

namespace MainProject.Controllers
{
    public class MainController : Controller
    {
        private IWebHostEnvironment _env;

        [HttpGet]
        [AllowAnonymous]
        public IActionResult homepage()
        {
            return View();
        }

        #region Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult register(UserDetails user, IFormFile Image)
        {
            ModelState.Remove("Photo");

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid input!";
                ViewData["MsgType"] = "warning";
                return View(user);
            }
            else
            {
                string picfilename = UploadFile(Image);
                //Image = UploadFile(Image);
                string sql = @"INSERT INTO UserDetails(UserName, Email, Password, Image)
                               VALUES('{0}','{1}','{2}','{3}')";
                string insert = String.Format(sql, user.UserName, user.Email, user.Password, picfilename);

                int count = DBUtl.ExecSQL(insert);
                if (count == 1)
                {
                    TempData["Message"] = $"User {user.UserName} Successfully Added.";
                    TempData["MsgType"] = "success";
                    return RedirectToAction("profile");
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                    return View(user);
                }

            }

        }

        #endregion

        #region profile
        [HttpGet]
        [Authorize]
        public IActionResult profile(string name)
        {
            string sql = String.Format("SELECT UserName FROM UserDetails WHERE UserName = '{0}'", name);
            DataTable dt = DBUtl.GetTable(sql);
            if (dt.Rows.Count == 1)
            {
                UserDetails view = new UserDetails
                {
                    UserName = (string)dt.Rows[0]["Name"],
                };
                return View(view);
            }
            return View("profile");
        }
        #endregion
        #region Update
        [HttpGet]
        [Authorize]
        public IActionResult update()
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
                return RedirectToAction("Profile");
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult upload(UserDetails user, IFormFile photo)
        {

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "danger";
                return View(user);
            }
            else
            {

                string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                string picfilename = UploadFile(user.Photo);

                string sql = @"UPDATE UserDetails
                           SET Email   = '{1}',
                               Password = '{2}',
                               Image = '{3}'
                         WHERE UserName = '{0}'";

                if (DBUtl.ExecSQL(sql, userid, user.Email, user.Password, picfilename) == 1)
                {
                    TempData["Message"] = $"User{userid} Successfully Updated";
                    TempData["MsgType"] = "success";
                    return RedirectToAction("Profile");
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
        #region Upload
        [HttpGet]
        [Authorize]
        public IActionResult upload()
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
                return RedirectToAction("Profile");
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult update(UserDetails user, IFormFile photo)
        {

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "danger";
                return View(user);
            }
            else
            {

                string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                string picfilename = UploadFile(user.Photo);

                string sql = @"UPDATE UserDetails
                           SET Email   = '{1}',
                               Password = '{2}',
                               Image = '{3}'
                         WHERE UserName = '{0}'";

                if (DBUtl.ExecSQL(sql, userid, user.Email, user.Password, picfilename) == 1)
                {
                    TempData["Message"] = $"User{userid} Successfully Updated";
                    TempData["MsgType"] = "success";
                    return RedirectToAction("Profile");
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
        #region trends
        [HttpGet]
        public IActionResult trends()
        {
            return View();
        }
        [HttpPost]
        public IActionResult trendsPost()
        {
            IFormCollection form = HttpContext.Request.Form;
            string trends = form["Trends"].ToString();
            if (ValidUtl.CheckIfEmpty(trends))
            {
                ViewData["Message"] = "Please enter all fields";
                ViewData["MsgType"] = "warning";
                return View("trends");
            }

            return RedirectToAction("Chart");
        }
        #endregion

        public IActionResult Chart()
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string sql = @"SELECT ROUND(AVG(Anger),3) AS Anger, ROUND(AVG(Contempt),3) AS Contempt, 
                            ROUND(AVG(Disgust),3) AS Disgust, ROUND(AVG(Fear),3) As Fear, 
                            ROUND(AVG(Happiness),3) As Happiness, ROUND(AVG(Neutral),3) AS Neutral, 
                            ROUND(AVG(Sadness),3) As Sadness, ROUND(AVG(Surprise),3) As Surprise 
                         FROM EmotionUserData
                         WHERE username = '{0}'";

            List<Chart> list = DBUtl.GetList<Chart>(sql, userid);

            if (list != null)
            {
                Chart crt = list[0];
                double[] dataEmotion = new double[]{crt.Anger, crt.Contempt, crt.Disgust, crt.Fear,
                                             crt.Happiness, crt.Neutral, crt.Sadness, crt.Surprise };
                ViewData["Data"] = dataEmotion;
            }
            else
            {
                TempData["Message"] = "Record does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("trends");
            }

            string[] colors = new[] { "cyan", "lightgreen", "yellow", "lightgrey", "pink", "grey", "blue", "green" };
            string[] grades = new[] { "Anger", "Contempt", "Disgust", "Fear", "Happiness", "Neutral", "Sadness", "Surprise" };
            ViewData["Chart"] = "pie";
            ViewData["Title"] = "Emotion Summary";
            ViewData["ShowLegend"] = "true";
            ViewData["Legend"] = "Cadets";
            ViewData["Colors"] = colors;
            ViewData["Labels"] = grades;

            return View("Chart");
        }

        public string UploadFile(IFormFile image)
        {
            string fext = Path.GetExtension(image.FileName);
            string uname = Guid.NewGuid().ToString();
            string fname = uname + fext;
            string fullpath = Path.Combine(_env.WebRootPath, "candidates/" + fname);
            FileStream fs = new FileStream(fullpath, FileMode.Create);
            image.CopyTo(fs);
            fs.Close();
            return fname;

        }

        public MainController(IWebHostEnvironment environment)
        {
            _env = environment;
        }
    }
}
