using System.ComponentModel.DataAnnotations;
using System;

namespace catalogueService.Database.DBsets
{
    public class sales
    {
        [Key]
        public int saleId { get; set; }
        public DateTime datePaid { get; set; }
        public string salesType { get; set; }
        public decimal amount { get; set; }
        public string customerID { get; set; }
    }
}
