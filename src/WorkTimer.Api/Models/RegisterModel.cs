using System.ComponentModel.DataAnnotations;

namespace WorkTimer.Api.Models {
    public class RegisterModel : LoginModel {
        [Required]
        public string PasswordConfirm { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
