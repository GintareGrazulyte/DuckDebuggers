﻿using System.ComponentModel.DataAnnotations;

namespace EShop.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
    }
}