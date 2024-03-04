using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NBP_backend.Models
{
    public class Product
    {
        
        public int ID { get; set; }

     

        public String Name { get; set; }

        public int Reviews { get; set; }

        public int GoodReviews { get; set; }

        public string Picture { get; set; }

        public List<Market> Markets { get; set; }

        public Category Category
        {
            get; set;
        } 

        public Stored storedIn( Market Market, double Price, bool Sale, bool Available)
        {
           
            return null;
        }

    }
}
