using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Models
{
    public class Product:BaseEntity
    {
        [StringLength(255),Required]
        public string Title { get; set; }
        [Column(TypeName ="money")]
        public double DiscountPrice { get; set; }
        [Column(TypeName = "money")]
        public double Price { get; set; }
        [Column(TypeName = "money")]
        public double ExTax { get; set; }
        [StringLength(20)]
        public string Code { get; set; }
        public bool Aviability { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public int Count { get; set; }
        public string MainImage { get; set; }
        public string HoverImage { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> BrandId { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<ProductTag> ProductTags { get; set; }
        public IEnumerable<ProductImage> productImages { get; set; }

    }
}
