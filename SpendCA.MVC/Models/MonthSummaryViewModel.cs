using System;
using System.Collections.Generic;

namespace SpendCA.MVC.Models
{
    public class MonthSummaryViewModel
    {
        public string MonthDescription { get; set; }
        public Double MonthTotal { get; set; }
        public List<CategoryViewModel> Categories;
    }
}
