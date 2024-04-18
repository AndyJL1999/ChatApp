using ChatApp.DataAccess.Models;
using ChatApp.UI_Library.Models;
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
        public string CurrentUserId { get; set; }
        public ChannelModel Channel { get; set; }
        public List<string> Recipients { get; set; }
        public List<MessageModel> Messages { get; set; }
    }
}
