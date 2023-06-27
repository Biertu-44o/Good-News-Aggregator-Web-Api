using System.ComponentModel.DataAnnotations;

namespace Web_Api_Controllers.RequestModels
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
