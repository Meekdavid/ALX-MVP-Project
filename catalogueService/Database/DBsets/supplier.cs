using System.ComponentModel.DataAnnotations;

namespace catalogueService.Database.DBsets
{
    public class supplier
    {
        [Key]
        public int supplierId { get; set; }
        [StringLength(50)]
        public string companyName { get; set; }
        [Phone]
        public int phoneNumber { get; set; }
        public int locationId { get; set; }
        public location? _location { get; set; }
    }
}
