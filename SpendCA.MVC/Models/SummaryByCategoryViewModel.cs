using System;
using System.Collections.Generic;

namespace SpendCA.MVC.Models
{
    public class SummaryByCategoryViewModel
    {
        public string CategoryDescription { get; set; }
        public List<MonthViewModel> Months;
    }
}
