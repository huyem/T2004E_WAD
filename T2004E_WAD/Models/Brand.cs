using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace T2004E_WAD.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thông tin")]
        public string Name { get; set; }
     
        [Required(ErrorMessage = "Vui lòng nhập thông tin")]
        public string Describe { get; set; }

        
        public string Image { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}