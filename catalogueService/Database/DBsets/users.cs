using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace catalogueService.Database
{
    public class users
    {
        [Key]
        public int userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string userName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }
        public string wallet { get; set; }
        public string? role { get; set; }
        [Phone]
        public int phoneNumber { get; set; }
        public int locationId { get; set; }
        public location? _location { get; set; }
        [Required]
        public int typeId { get; set; }
        public type? _type { get; set; }
    }
}
