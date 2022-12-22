using System;
using System.ComponentModel.DataAnnotations;

namespace catalogueService.Models
{
    public class orderModel
    {
        public int orderID { get; set; }
        public DateTime dateReceived { get; set; }
        public DateTime delivered { get; set; }
        [Required]
        public int productId { get; set; }
        public string discountID { get; set; }
        public string paymentID { get; set; }
        public decimal amount { get; set; }
        public int quantity { get; set; }
        public string orderStatus { get; set; }
        public int customerId { get; set; }
    }
}
