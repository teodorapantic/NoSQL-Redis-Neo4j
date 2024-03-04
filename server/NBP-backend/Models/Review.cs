using System;

namespace NBP_backend.Models
{
    public class Review
    {
        public int ReturnID { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }

        public bool Recommend { get; set; }

        public DateTime date { get; set; }
    }
}
