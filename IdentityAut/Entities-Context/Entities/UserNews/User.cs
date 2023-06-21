using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Core;

namespace Entities_Context.Entities.UserNews
{
    public class User
    {

        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String? ProfilePicture { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public Int32 PositiveRate { get; set; }
        public DateTime Created { get; set; }

        public Int32 PositiveRateFilter { get; set; }

        public List<Comment>? Comments { get; set; }

        public List<UsersRoles> Role { get; set; }

        public SiteTheme Theme { get; set; }
        public Int32 ThemeId { get; set; }
    }
}
