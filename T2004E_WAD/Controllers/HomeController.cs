using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using T2004E_WAD.Context;
using T2004E_WAD.Models;

namespace T2004E_WAD.Controllers
{
    public class HomeController : Controller
    {
   


   

            private DataContext db = new DataContext();
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

            public string CheckOutSuccess()
            {
                return "Tạo đơn thành công...";
            }


      
    }


}
