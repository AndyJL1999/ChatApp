using ChatApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Models
{
    public class Chat : IChannel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
