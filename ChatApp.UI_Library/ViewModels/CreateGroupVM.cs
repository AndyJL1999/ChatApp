using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.UI_Library.ViewModels
{
    public class CreateGroupVM
    {
        [Required]
        [StringLength(15)]
        [DisplayName("Group Name")]
        public string GroupName { get; set; }

        public List<string> PhoneNumbers { get; set; }
    }
}
