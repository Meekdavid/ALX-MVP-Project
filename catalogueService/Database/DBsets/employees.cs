using System;
using System.ComponentModel.DataAnnotations;

namespace catalogueService.Database
{
    public class employees
    {
        [Key]
        public int employeeId { get; set; }
        [StringLength(50)]
        public string firstName { get; set; }
        [StringLength(50)]
        public string lastName { get; set; }
        [StringLength(50)]
        [EmailAddress]
        public string email { get; set; }
        public DateTime hiredDate { get; set; }
        public int locationId { get; set; }
        public location? _location { get; set; }
        public int jobId { get; set; }
        public job? _job { get; set; }
    }
}
