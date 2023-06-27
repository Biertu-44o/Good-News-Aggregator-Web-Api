using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Account
{
    public class TokenDto
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
