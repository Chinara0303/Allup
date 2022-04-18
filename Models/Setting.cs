using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Models
{
    public class Setting: BaseEntity
    {

        [StringLength(255), Required]
        public string Offer { get; set; }

        [StringLength(255), Required]
        public string Logo { get; set; }

        [StringLength(255), Required]
        public string SupportPhone { get; set; }

        [StringLength(255), Required]
        public string Phone { get; set; }

        [StringLength(255), Required]
        public string Email { get; set; }

        [StringLength(255), Required]
        public string WorkDay { get; set; }
       

    }
}
