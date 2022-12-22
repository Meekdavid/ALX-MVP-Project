using System;

namespace catalogueService.Models
{
    public class saleModel
    {
        public DateTime datePaid { get; set; }
        public string salesType { get; set; }
        public decimal amount { get; set; }
        public string customerID { get; set; }
    }
}
