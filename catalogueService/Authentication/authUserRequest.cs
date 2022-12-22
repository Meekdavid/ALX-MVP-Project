using System.ComponentModel.DataAnnotations;

namespace catalogueService.Authentication
{
    public class authUserRequest
    {
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        [EmailAddress]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public int phoneNumber { get; set; }
        public int locationId { get; set; }
        public int typeId { get; set; }
    }
}
