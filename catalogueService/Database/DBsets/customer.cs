using System.ComponentModel.DataAnnotations;

namespace catalogueService.Database
{
    public class customer
    {
        [Key]
        public int customerId { get; set; }
        [StringLength(50)]
        public string firstName { get; set; }
        [StringLength(50)]
        public string lastName { get; set; }
        [StringLength(11)]
        [Phone]
        public int phoneNumber { get; set; }
        public int userId { get; set; }
        public users? _users { get; set; }
    }
}
