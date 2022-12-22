using catalogueService.Database;
using System.ComponentModel.DataAnnotations;
using System;

namespace catalogueService.Models
{
    public class employeeModel
    {

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public DateTime hiredDate { get; set; }
        public int locationId { get; set; }
        public int jobId { get; set; }
    }
}
