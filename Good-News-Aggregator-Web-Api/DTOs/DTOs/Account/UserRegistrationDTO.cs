using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Account
{
    public class UserRegistrationDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

    }
}
