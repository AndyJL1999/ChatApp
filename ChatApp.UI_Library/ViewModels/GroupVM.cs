using ChatApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.UI_Library.ViewModels
{
    public class GroupVM
    {
        public string PhoneNumber { get; set; }
        public IEnumerable<string>? Users { get; set; }
    }
}
