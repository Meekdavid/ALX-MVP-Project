using System.ComponentModel.DataAnnotations;

namespace catalogueService.Models
{
    public class userModel
    {
        public int userId { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        [EmailAddress]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string role { get; set; }
        public int phoneNumber { get; set; }
        public string wallet { get; set; }
        public int locationId { get; set; }
        public int typeId { get; set; }
    }
}
