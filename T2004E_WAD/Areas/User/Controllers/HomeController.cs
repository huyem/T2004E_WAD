using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using T2004E_WAD.Context;
using T2004E_WAD.Models;

namespace T2004E_WAD.Areas.User.Controllers
{
    public class HomeController : Controller
    {

        private DataContext db = new DataContext();

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

                if (check == null)
                {
                    user.Password = GetMD5(user.Password);
                    db.Users.Add(user);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    return RedirectToAction("RegisterSucces");
                   
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
            for (int i = 0; i < tgData.Length; i++)
            {
                hashString += tgData[i].ToString("x2");
            }
            return hashString;
        }
        public ActionResult RegisterSucces()
        {
            ViewBag.error = "Đăng kí thành công...";
            return RedirectToAction("Index");
        }

        public ActionResult Login(string ReturUrl)
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

            if (data.Count > 0)
            {
                Session.Add(Constant.USER_SESSION,email);
                // login thanh cong 
                var u = data.FirstOrDefault();
                FormsAuthentication.SetAuthCookie(u.Email, true);
                return RedirectToAction("Index");
            }
            else { ModelState.AddModelError("", "Thông tin đăng nhập sai!"); }
            return View();

        }

        public ActionResult LogOut()
        {
            if (Session[Constant.USER_SESSION] == null) ;


            return RedirectToAction("Index","Home");
        }
        // GET: User/Home
        public ActionResult Index()
        {
         

            var products = db.Products.ToList();  
          
           
            return View(products);
        }

        // GET: User/Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Home/Create
        public ActionResult Create()
        {
            return View();
        }




        public ActionResult AddToCart(int? id, int? qty)
        {
            try
            {
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                // them vao gio hang
                CartItem item = new CartItem(product, (int)qty);
                // lay gio hang tu Session
                Cart cart = (Cart)Session["Cart"];
                if (cart == null)
                {
                    Customer customer = new Customer("Nguyễn Văn An", "0987654321", "Số 8 Tôn Thất Thuyết");
                    cart = new Cart();
                    cart.Customer = customer;
                }
                cart.AddToCart(item);
                //phai them cart vao session
                Session["cart"] = cart;
            }
            catch (Exception e)
            {
                _ = e.StackTrace;
                return HttpNotFound();
            }
            return RedirectToAction("Cart");
        }

        public ActionResult Cart()
        {
            return View();
        }



        public ActionResult RemoveItem(int? id)
        {
            try
            {

                Cart cart = (Cart)Session["Cart"];
                if (cart == null)
                {
                    return HttpNotFound();
                }
                cart.RemoveItem((int)id);
                Session["cart"] = cart;// theem session
            }
            catch (Exception e)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Cart");


        }

        // POST: User/Home/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CheckOut()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckOut(Order order)
        {
            if (ModelState.IsValid)
            {
                var cart = (Cart)Session["cart"];
                order.GrandTotal = cart.GrandTotal;
                order.CreatedAt = DateTime.Now;
                order.Status = 1;
                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var item in cart.CartItems)
                {
                    OrderItem orderItem = new OrderItem() { OrderID = order.Id, ProductID = item.Product.Id, Qty = item.Quantity, Price = item.Product.Price };
                    db.OrderItems.Add(orderItem);
                }
                db.SaveChanges();
                Session["cart"] = null;// xoa gio hang
            }

            return RedirectToAction("CheckOutSuccess");
        }

        public ActionResult CheckOutSuccess()
        {
          
            return RedirectToAction("Index");
        }


    }
}
