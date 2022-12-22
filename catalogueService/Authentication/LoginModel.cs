using System.ComponentModel.DataAnnotations;

namespace catalogueService.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string userName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }
    }
}
