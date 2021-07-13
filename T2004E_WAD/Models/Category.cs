using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace T2004E_WAD.Models
{
    public class Category
    {
        [Key] // khai bao day la khoa chinh
        public int Id { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập tên danh mục")]
        public string Name { get; set; }

       
        public string Image { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string Description { get; set; }
       
        public virtual ICollection<Product> Products { get; set; } 
    }
}