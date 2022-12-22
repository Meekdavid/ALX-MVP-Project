using System.ComponentModel.DataAnnotations;

namespace catalogueService.Database
{
    public class job
    {
        [Key]
        public int jobId { get; set; }
        [StringLength(50)]
        public string jobTitle { get; set; }
        public decimal salary { get; set; }
    }
}
