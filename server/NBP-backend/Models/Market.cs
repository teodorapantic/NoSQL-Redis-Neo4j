using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace NBP_backend.Models
{
    public class Market
    {
        
        public int ID { get; set; }
        
        public String Name { get; set; }

         public List<Stored> StoredProducts
        {
            get; set;
        }

        public List<Category> Categories
        {
            get; set;
        }


    }
}
