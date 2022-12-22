using System.ComponentModel.DataAnnotations;

namespace catalogueService.Database
{
    public class type
    {
        [Key]
        public int typeId { get; set; }
        [StringLength(50)]
        public string userType { get; set; }
    }
}
