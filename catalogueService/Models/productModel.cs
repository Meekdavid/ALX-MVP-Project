using catalogueService.Database;
using catalogueService.Database.DBsets;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace catalogueService.Models
{
    public class productModel
    {
        public int productId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public int categoryId { get; set; }
    }
}
