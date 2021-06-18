using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace T2004E_WAD.Models
{
    public class Product
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Qty { get; set; }
        [Required]
        public string Price { get; set; }
        [Required]
        public string Image { get; set; }
    }
}