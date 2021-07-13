using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using T2004E_WAD.Context;
using T2004E_WAD.Models;
using System.Dynamic;
using System.IO;

namespace T2004E_WAD.Controllers
{
    public class ProductsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Products
        public ActionResult Index(string search,string sortOder)
        {
            ViewBag.CategoryId = 0;
                        var product = db.Products.Include(p => p.Brand).Include(p => p.Category);

            string sort = !String.IsNullOrEmpty(sortOder) ? sortOder : "asc";
            if (!String.IsNullOrEmpty(search))
            {
                var products = db.Products.Where(p => p.Name.Contains(search)).ToList();
                return View(products);
            }

            switch (sort)
            {                           
                case "asc":
                
                     product = db.Products.Include(p => p.Brand).Include(p => p.Category).OrderBy(p => p.Name);                    
                    break;
                case "desc":
                     product = db.Products.Include(p => p.Brand).Include(p => p.Category).OrderByDescending(p => p.Name);
                    break;
            }
            //if (!String.IsNullOrEmpty(categoryId))
            //{
            //    var catId = Convert.ToInt32(categoryId);
            //    product = product.Where(p => p.CategoryId == catId);
            //    ViewBag.CategoryId = catId;

            //}
            //var category = db.Categories.ToList();
            //dynamic data = new ExpandoObject();
            //data.Products = product;
            //data.Categories = category;
      
            return View(product);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
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
        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

  

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Qty,Price,Describe,CategoryId,BrandId,UploadFI")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", product.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", product.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Qty,Price,Image,Describe,CategoryId,BrandId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", product.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
                db.SaveChanges() ;
               
                foreach(var item in cart.CartItems)
                {
                    OrderItem orderItem = new OrderItem() { OrderID = order.Id, ProductID = item.Product.Id, Qty = item.Quantity, Price = item.Product.Price };
                    db.OrderItems.Add(orderItem);
                }
                db.SaveChanges();
                Session["cart"] = null;// xoa gio hang
            }

            return RedirectToAction("CheckOutSuccess");
        }

        public string CheckOutSuccess()
        {
            return "Tạo đơn thành công...";
        }


        //client
        
     
    }
}
