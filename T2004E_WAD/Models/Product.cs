using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace T2004E_WAD.Models
{
    public class Product
    {
      
        [Key]
        public int Id { get; set; }
      
        [Required(ErrorMessage ="Vui lòng nhập tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public string Qty { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
        public int Price { get; set; }

        
        public string Image { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thông tin")]
        public string Describe { get; set; }

        [Required(ErrorMessage = "Vui lòng phân loại cho sản phẩm")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
        public int BrandId { get; set; }

       

        public virtual Category Category { get; set; }

        public virtual Brand Brand { get; set; }

       
    }
}