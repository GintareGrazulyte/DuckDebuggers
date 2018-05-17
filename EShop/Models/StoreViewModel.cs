using BOL;
using BOL.Objects;
using System.Collections.Generic;

namespace EShop.Models
{
    public class StoreViewModel
    {
        public List<Category> DisplayedCategories { get; set; }
        public List<Item> DisplayedItems { get; set; }
        public List<Item> AllItems { get; set; }
    }
}