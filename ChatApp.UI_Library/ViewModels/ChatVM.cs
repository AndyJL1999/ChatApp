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
    public class ChatVM
    {
        public ChannelModel Channel { get; set; }
        public RecipientModel Recipient { get; set; }
        public List<MessageModel> Messages { get; set; }

    }
}
