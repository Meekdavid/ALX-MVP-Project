using System.ComponentModel.DataAnnotations;

namespace catalogueService.Models
{
    public class supplierModel
    {
        public int supplierId { get; set; }
        public string companyName { get; set; }
        public int phoneNumber { get; set; }
        public int locationId { get; set; }
    }
}
