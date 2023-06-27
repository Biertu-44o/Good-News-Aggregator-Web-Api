using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities_Context.Entities.UserNews
{
    public class RefreshToken
    {
        public Int32 Id { get; set; }
        public Guid Value { get; set; }
        public Int32 UserId { get; set; }
        public User User { get; set; }
    }
}
