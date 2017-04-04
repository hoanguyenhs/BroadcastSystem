using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroadcastSystem.Models
{
    public class MessageModel
    {
        public int Id { get; set; }       
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdateOn { get; set; }



    }
}