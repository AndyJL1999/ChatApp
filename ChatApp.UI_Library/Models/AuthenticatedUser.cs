using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.UI_Library.Models
{
    public class AuthenticatedUser
    {
        public string Name { get; set; }
        internal string Token { get; set; }
    }
}
