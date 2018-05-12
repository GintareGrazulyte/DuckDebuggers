using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EShop.Models
{
    public class DiscountViewModel
    {
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public decimal Value { get; set; }
        public DiscountType DiscountType { get; set; }
        public IEnumerable<int> ItemIds { get; set; }
        public string Items { get; set; }
    }

    public enum DiscountType
    {
        Absolute,
        Percentage
    }
}