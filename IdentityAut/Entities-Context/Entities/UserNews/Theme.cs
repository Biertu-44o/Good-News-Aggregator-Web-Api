using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Entities_Context.Entities.UserNews
{
    public class SiteTheme  
    {
        public Int32 Id { get; set; }
        public String Theme { get; set; }

        public List<User> Users { get; set; }
    }
}
