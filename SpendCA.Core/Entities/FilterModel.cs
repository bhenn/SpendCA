using System;
using System.Collections.Generic;

namespace SpendCA.Core.Entities
{
    public class FilterModel
    {
        public DateTime MinDate { get; set; }
        public DateTime MaxDate{ get; set; }
        public List<int> SelectedCategories { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
