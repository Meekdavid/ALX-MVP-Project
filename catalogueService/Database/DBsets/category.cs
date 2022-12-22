using System.ComponentModel.DataAnnotations;

namespace catalogueService.Database
{
    public class category
    {
        [Key]
        public int categoryId { get; set; }
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(100)]
        public string description { get; set; }

    }
}
