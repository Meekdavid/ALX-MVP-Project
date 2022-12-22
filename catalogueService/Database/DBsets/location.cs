using System.ComponentModel.DataAnnotations;

namespace catalogueService.Database
{
    public class location
    {
        [Key]
        public int locationId { get; set; }
        [StringLength(100)]
        public string province { get; set; }
        [StringLength(50)]
        public string city { get; set; }
        [StringLength(100)]
        public string street { get; set; }
    }
}
