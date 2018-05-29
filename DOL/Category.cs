using BOL.Objects;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    public class Category
    {
        public Category()
        {
            Properties = new HashSet<BOL.Property.Property>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Incorrect category name")]
        public string Name { get; set; }
        public List<Item> Items { get; set; }

        public virtual ICollection<BOL.Property.Property> Properties { get; set; }
    }
}
