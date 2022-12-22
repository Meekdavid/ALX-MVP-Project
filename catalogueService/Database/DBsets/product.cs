using catalogueService.Database.DBsets;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace catalogueService.Database
{
    public class product
    {
        [Key]
        public int productId { get; set; }
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(150)]
        public string description { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public int categoryId { get; set; }
        public category? _category { get; set; }
    }
}
