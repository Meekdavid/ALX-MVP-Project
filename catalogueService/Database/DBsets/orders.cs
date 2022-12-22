using catalogueService.Database.DBsets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace catalogueService.Database.DBsets
{
    public class orders
    {
        [Key]
        public int orderID { get; set; }
        [Required]
        public int productId { get; set; }
        public DateTime dateReceived { get; set; }
        public DateTime delivered { get; set; }
        public string discountID { get; set; }
        public string paymentID { get; set; }
        public decimal amount { get; set; }
        public int quantity { get; set; }
        public string orderStatus { get; set; }
        public int customerId { get; set; }
        public customer? _customer { get; set; }
    }
}
