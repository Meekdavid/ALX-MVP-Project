using catalogueService.Database;
using catalogueService.Database.DBsets;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace catalogueService.Models
{
    public class customerModel
    {
        public int customerId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int phoneNumber { get; set; }
        public int userId { get; set; }
    }
}
