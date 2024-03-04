using System;

namespace NBP_backend.HelperClasses
{
    public class ProductSerialization
    {
        public int ID { get; set; }

        public String Name { get; set; }

        public int Reviews { get; set; }

        public int GoodReviews { get; set; }

        public string Picture { get; set; }

        public string Manufacturer { get; set; }
    }
}
