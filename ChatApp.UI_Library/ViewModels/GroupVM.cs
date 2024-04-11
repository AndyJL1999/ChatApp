using ChatApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.UI_Library.ViewModels
{
    public class GroupVM
    {
        [Required]
        [DisplayName("Group Name")]
        public string GroupName { get; set; }

        [Phone(ErrorMessage = "Enter valid phone number")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public List<string> PhoneNumbers { get; set; } 
    }
}
