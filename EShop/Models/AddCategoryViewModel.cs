using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShop.Models
{
    public class AddCategoryViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public List<CheckBoxListItem> Properties { get; set; }

        public AddCategoryViewModel()
        {
            Properties = new List<CheckBoxListItem>();
        }
    }
}