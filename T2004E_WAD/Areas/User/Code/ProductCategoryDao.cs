using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using T2004E_WAD.Context;

namespace T2004E_WAD.Areas.User.Code
{
    public class ProductCategoryDao
    {
        DataContext db = null;
        public ProductCategoryDao()
        {
            db = new DataContext();
        }
        public List<Models.Category> ListAll()
        {
            return db.Categories.ToList();
        }
    }
}