using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Models
{
    public class UserChat
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ChatId { get; set; }
    }
}
