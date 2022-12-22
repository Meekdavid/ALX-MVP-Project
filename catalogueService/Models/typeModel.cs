using System.ComponentModel.DataAnnotations;

namespace catalogueService.Models
{
    public class typeModel
    {
        public int typeId { get; set; }
        [StringLength(50)]
        public string userType { get; set; }
    }
}
