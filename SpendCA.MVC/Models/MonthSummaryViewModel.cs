using System;
using System.Collections.Generic;

namespace SpendCA.MVC.Models
{
    public class MonthSummaryViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthDescription { get; set; }
        public Double MonthTotal { get; set; }
        public List<CategoryViewModel> Categories;
    }
}
