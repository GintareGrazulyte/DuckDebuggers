using BOL.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.Discounts
{
    public class Discount
    {
        public Discount()
        {
            Items = new HashSet<Item>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime BeginDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
