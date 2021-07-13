using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Text;
using System.Web.Mvc;
using T2004E_WAD.Context;
using T2004E_WAD.Models;
using System.Security.Cryptography;


namespace T2004E_WAD.Areas.User.Controllers
{
    public class UserController : Controller
    {
       
        private DataContext db = new DataContext();
        // GET: User/Uesr

        [Authorize]

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Register(Models.User user)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.FirstOrDefault(s => s.Email == user.Email);
            
                if(check == null)
                {
                    user.Password = GetMD5(user.Password);
                    db.Users.Add(user);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.error = "Email already exists";
            }
            return View();

        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] frData = Encoding.UTF8.GetBytes(str);
            byte[] tgData = md5.ComputeHash(frData);
            string hashString = "";
            for(int i=0; i< tgData.Length; i++)
            {
                hashString += tgData[i].ToString("x2");
            }
            return hashString;
        }


        public ActionResult Login( string ReturUrl)
        {
            ViewBag.ReturUrl = ReturUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Login(string email, string password)
        {
            var f_password = GetMD5(password);
            var data = db.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
       
            if( data.Count > 0)
            {
                // login thanh cong 
                var u = data.FirstOrDefault();
                FormsAuthentication.SetAuthCookie(u.Email, true);
                return RedirectToAction("Index");
            }
            return View();
        
        }
    }
}