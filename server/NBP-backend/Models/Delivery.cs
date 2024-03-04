using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBP_backend.Models
{
    public class Delivery
    {

        public String Name { get; set; }

        public String Password { get; set; }

        public int DeliveryCost { get; set; }

        public List<OrderProduct> Orders { get; set; }


    }
}
