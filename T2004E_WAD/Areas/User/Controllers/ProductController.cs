using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using T2004E_WAD.Areas.User.Code;

namespace T2004E_WAD.Areas.User.Controllers
{
    public class ProductController : Controller
    {
        // GET: User/Product
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult ListCategory()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }
    }
}