using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBP_backend.Models
{
    public class OrderProduct
    {

        public String  UserName { get; set; }

        public String MarketName { get; set; }

        public String ProductName { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public DateTime Time { get; set; }

        public String Location { get; set; }

        public String PhoneNumber { get; set; }

        public bool Delivered { get; set; }
    }
}
