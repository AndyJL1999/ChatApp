using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.UI_Library.ViewModels
{
    public class ChatVM
    {
        [Phone(ErrorMessage = "Enter valid phone number")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
