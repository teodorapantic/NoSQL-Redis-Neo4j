using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NBP_backend.Models
{
    public class User
    {
        public int returnID { get; set; }

        public String Name { get; set; }

        public String Surname { get; set; }

        public String UserName { get; set; }

        public String Password
        {
            get; set;
        } 

        public string Location { get; set; }

        public string PhoneNumber { get; set; }

        public List<Product> Follow { get; set; }

        public  List<Product> Searched { get; set; }


    }
}
