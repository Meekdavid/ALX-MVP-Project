using System.ComponentModel.DataAnnotations;

namespace catalogueService.Database
{
    public class manager
    {
        [Key]
        public int managerId { get; set; }
        [StringLength(50)]
        public string firstName { get; set; }
        [StringLength(50)]
        public string lastName { get; set; }
        [StringLength(50)]
        [EmailAddress]
        public string email { get; set; }
        [Phone]
        public int phoneNumber { get; set; }
        public int locationId { get; set; }
        public location? _location { get; set; }
    }
}
