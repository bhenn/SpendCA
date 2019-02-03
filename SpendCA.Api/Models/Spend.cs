using System;

namespace SpendCA.Models
{
    public class Spend : Base
    {
        public string Description { get; set; }
        public string Location { get; set; }
        public long Value { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}