using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBP_backend.Models
{
    public class Notification
    {
        public int ProductID { get; set; }

        public string Market { get; set; }

        public DateTime Time { get; set; }

        public string Text  { get; set; }


    }
}
