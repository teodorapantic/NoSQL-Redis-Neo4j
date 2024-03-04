using NBP_backend.Models;
using System;
using System.Collections.Generic;

namespace NBP_backend.HelperClasses
{
    public class ProductSerializationRedis
    {
        public int IdProduct { get; set; }

        public String NameProduct { get; set; }

        public int Reviews { get; set; }

        public int GoodReviews { get; set; }    

        public int Rank { get; set; }

        public string PictureProduct { get; set; }

        public string Manufacturer { get; set; }

        public List<Stored> Stored { get; set; }    
    }
}
